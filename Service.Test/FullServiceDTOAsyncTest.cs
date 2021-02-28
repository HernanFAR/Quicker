using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Mapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Services.Test
{
    public class FullServiceDTOAsyncTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly IMapper _Mapper;
        private readonly FullServiceAsync<int, TestModelRelation, TestModelRelationDTO> _Service;

        public FullServiceDTOAsyncTest()
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();
            _Mapper = new MapperConfiguration(config =>
            {
                config.AddProfile<TestModelRelationMapper>();
            })
            .CreateMapper();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration(config =>
            {
                config.UseLogger = true;
                config.UseAutoMapper = true;
            });
            container.AddScoped<DbContext, TestContext>(e => _Context);
            container.AddScoped(e => _Mapper);

            _Service = new Fake.FakeFullServiceDTO(container.BuildServiceProvider());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("12")]
        [InlineData("1234567891012131415")]
        public void ValidateObjectBeforeUpdating_Failure_InvalidNameValues(string name)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeUpdating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelationDTO
            {
                Name = name,
                TestModelId = 1
            };


            // Act
            TargetInvocationException invEx = null;
            try
            {
                method.Invoke(_Service, new object[] { model });
            }
            catch (TargetInvocationException e)
            {
                invEx = e;
            }

            // Asserttion
            if (invEx.InnerException is ValidationException ex)
            {
                bool hasName = ex.Errors.Any(e => e.PropertyName == "Name");

                Assert.True(hasName);
            }
            else
            {
                Assert.False(true);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ValidateObjectBeforeUpdating_Failure_InvalidTestModelIdValues(int testModelId)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeUpdating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = testModelId
            };


            // Act
            TargetInvocationException invEx = null;
            try
            {
                method.Invoke(_Service, new object[] { model });
            }
            catch (TargetInvocationException e)
            {
                invEx = e;
            }

            // Assertion
            if (invEx.InnerException is ValidationException ex)
            {
                bool hasName = ex.Errors.Any(e => e.PropertyName == "TestModelId");

                Assert.True(hasName);
            }
            else
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void ValidateObjectBeforeUpdating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeUpdating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 1
            };


            // Act
            method.Invoke(_Service, new object[] { model });
            

            // Asserttion
            Assert.False(false);
        }

        [Fact]
        public void PresetPropertiesBeforeUpdating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("PresetPropertiesBeforeUpdating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelation
            {
                Name = "Test1",
                TestModelId = 1
            };


            // Act
            method.Invoke(_Service, new object[] { model, model });
            

            // Asserttion
            Assert.False(false);
        }

        [Fact]
        public async Task Update_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            string expMessage = "entity";

            // Act - Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _Service.Update(1, null));

            Assert.Contains(expMessage, ex.Message);
        }

        [Fact]
        public async Task Update_Failure_ShouldThrowValidationException()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "TestModel", Percent = 10 });
            var entry = _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "TestModelRelation", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Id = entry.Entity.Id,
                Name = "12",
                TestModelId = 1
            };

            // Act - Assert
            await Assert.ThrowsAsync<ValidationException>(() => _Service.Update(model.Id, model));
        }

        [Fact]
        public async Task Update_Failure_ShouldThrowInvalidOperationException_MessageEqualToKey()
        {
            // Arrange
            int unexpKey = 1;
            string expMessage = "key";
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "TestModel", Percent = 10 });
            var entry = _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "TestModelRelation", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 1
            };

            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _Service.Update(unexpKey, model));

            Assert.Equal(expMessage, ex.Message);
        }

        [Fact]
        public async Task Update_Success()
        {
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "TestModel", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Id = 2, Name = "TestModel", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "TestModelRelation", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            // Arrange
            var model = new TestModelRelationDTO
            {
                Id = 1,
                Name = "Test2",
                TestModelId = 2
            };

            // Act
            var created = await _Service.Update(model.Id, model);

            // Assert
            Assert.NotNull(created);

            var finded = await _Context.TestModelRelations.FindAsync(1);

            Assert.Equal(model.Name, finded.Name);
            Assert.Equal(model.TestModelId, finded.TestModelId);
        }
    }
}
