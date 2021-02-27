using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using Quicker.Test.Mapper;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Quicker.Test.Services
{
    public class OpenServiceDTOAsyncTest : IDisposable
    {
        private TestContext _Context;
        private IMapper _Mapper;
        private OpenServiceAsync<int, TestModelRelation, TestModelRelationDTO> _Service;

        public OpenServiceDTOAsyncTest()
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

            _Service = new FakeServices.FakeOpenServiceDTO(container.BuildServiceProvider());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("12")]
        [InlineData("1234567891012131415")]

        public void ValidateObjectBeforeCreating_Failure_InvalidNameValues(string name)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
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

        public void ValidateObjectBeforeCreating_Failure_InvalidTestModelIdValues(int testModelId)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
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

            // Asserttion
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
        public void ValidateObjectBeforeCreating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 2
            };


            // Act
            method.Invoke(_Service, new object[] { model });
            

            // Asserttion
            Assert.False(false);
        }

        [Fact]
        public void PresetPropertiesBeforeCreating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("PresetPropertiesBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 2
            };


            // Act
            method.Invoke(_Service, new object[] { model });
            

            // Asserttion
            Assert.False(false);
        }

        [Fact]
        public async Task Create_Failure_ShouldThrowValidationException()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Name = "12",
                TestModelId = 1
            };

            // Act - Assert
            await Assert.ThrowsAsync<ValidationException>(() => _Service.Create(model));
        }

        [Fact]
        public async Task Create_Failure_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Id = 1,
                Name = "Test1",
                TestModelId = 1
            };

            // Act - Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _Service.Create(model));
        }

        [Fact]
        public async Task Create_Failure_ShouldThrowDbUpdateException_NotExistingRelation()
        {
            // Arrange
            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 1
            };

            // Act - Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _Service.Create(model));

        }

        [Fact]
        public async Task Create_Failure_ShouldThrowDbUpdateException_UniqueKeyViolation()
        {
            // Arrange
            var testModel = _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });
            var entry = _Context.TestModelRelations.Add(new TestModelRelation { 
                Name = "Test1", 
                UniqueName = "Unique",
                TestModelNavigation = testModel.Entity
            });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                UniqueName = "Unique",
                TestModelId = 1
            };

            // Act - Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _Service.Create(model));
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Name = "Test1",
                TestModelId = 1
            };

            // Act
            var created = await _Service.Create(model);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(1, _Context.TestModels.Count());
        }

        [Fact]
        public async Task Delete_WithEntityAsParameter_Failure_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _Service.Delete(
                    new TestModelRelationDTO { 
                        Id = 256 
                    }
                )
            );
        }

        [Fact]
        public void DeleteFilter_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("DeleteFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new TestModelRelation
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
        public async Task Delete_WithIdAsParameter_Failure_ShouldThrowInvalidOperationException()
        {
            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _Service.Delete(256)
            );
        }

        [Fact]
        public async Task Delete_WithIdAsParameter_Success()
        {
            // Arrange
            var testModel = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 20 });
            var entry = _Context.TestModelRelations.Add(
                new TestModelRelation { 
                    Name = "Test2", 
                    TestModelId = 0, 
                    TestModelNavigation = testModel.Entity
                }
            );

            await _Context.SaveChangesAsync();

            // Act
            await _Service.Delete(entry.Entity.Id);

            // Assert
            Assert.Equal(0, _Context.TestModelRelations.Count());
        }
    }
}
