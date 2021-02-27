using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using Quicker.Test.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Quicker.Test.Services
{
    public class OpenServiceAsyncTest : IDisposable
    {
        private TestContext _Context;
        private OpenServiceAsync<int, TestModel> _Service;

        public OpenServiceAsyncTest()
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection();

            // Se agrego la instancia como singleton
            container.AddSingleton<DbContext, TestContext>(e => _Context);

            _Service = new FakeServices.FakeOpenService(container.BuildServiceProvider());
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
        public void ValidateObjectBeforeCreating_Failure_InvalidPercentValues(int percent)
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
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
        public void ValidateObjectBeforeCreating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("ValidateObjectBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
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
        public void PresetPropertiesBeforeCreating_Success()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("PresetPropertiesBeforeCreating", BindingFlags.NonPublic | BindingFlags.Instance);
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
        public async Task Create_Failure_ShouldThrowValidationException()
        {
            // Arrange
            var model = new TestModel
            {
                Name = "12",
                Percent = 0
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

            var model = new TestModel
            {
                Id = 1,
                Name = "Test1",
                Percent = 10
            };

            // Act - Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _Service.Create(model));
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var model = new TestModel
            {
                Name = "Test1",
                Percent = 10
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

            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _Service.Delete(
                    new TestModel { 
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
        public async Task Delete_WithIdAsParameter_Failure_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            // Act - Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _Service.Delete(256)
            );
        }

        [Fact]
        public async Task Delete_WithIdAsParameter_Success()
        {
            // Arrange
            var entry = _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            // Act
            await _Service.Delete(entry.Entity.Id);

            // Assert
            Assert.Equal(0, _Context.TestModels.Count());
        }
    }
}
