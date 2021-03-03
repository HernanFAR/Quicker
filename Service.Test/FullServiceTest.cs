using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Repository;
using Xunit;

namespace Quicker.Services.Test
{
    public class FullServiceAsyncTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly FullServiceAsync<int, TestModel> _Service;

        public FullServiceAsyncTest()
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration(config =>
            {
                config.UseLogger = true;
                config.UseAutoMapper = true;
            });
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new Fake.FakeFullService(container.BuildServiceProvider());
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
            var model = new TestModel
            {
                Name = name,
                Percent = 10
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
        [InlineData(101)]
        public void ValidateObjectBeforeUpdating_Failure_InvalidPercentValues(int percent)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeUpdating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModel
            {
                Name = "Test1",
                Percent = percent
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
                bool hasName = ex.Errors.Any(e => e.PropertyName == "Percent");

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
            var model = new TestModel
            {
                Name = "Test1",
                Percent = 10
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
            var model = new TestModel
            {
                Name = "Test1",
                Percent = 10
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
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "TestModel", Percent = 10 });

            await _Context.SaveChangesAsync();

            var model = new TestModel
            {
                Id = entry.Entity.Id,
                Name = "12",
                Percent = 0
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
            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 20 });

            await _Context.SaveChangesAsync();

            var model = new TestModel
            {
                Name = "Test1",
                Percent = 10
            };

            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _Service.Update(unexpKey, model));

            Assert.Equal(expMessage, ex.Message);
        }

        [Fact]
        public async Task Update_Success()
        {
            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 20 });

            await _Context.SaveChangesAsync();

            // Arrange
            var model = new TestModel
            {
                Id = 1,
                Name = "Test2",
                Percent = 10
            };

            // Act
            var created = await _Service.Update(model.Id, model);

            // Assert
            Assert.NotNull(created);

            var finded = await _Context.TestModels.FindAsync(1);

            Assert.Equal(model.Name, finded.Name);
            Assert.Equal(model.Percent, finded.Percent);
        }
    }
}
