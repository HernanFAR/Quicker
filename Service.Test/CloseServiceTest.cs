﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Repository;
using Xunit;

namespace Quicker.Services.Test
{
    public class CloseServiceTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly CloseServiceAsync<int, TestModel> _Service;

        public CloseServiceTest() 
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration(config => {
                config.UseLogger = true;
                config.UseAutoMapper = true;
            });
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new Fake.FakeCloseService(container.BuildServiceProvider());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Query_Success_ShouldReturnEmptyIQueryriable()
        {
            // Arrange
            int expCount = 0;
            MethodInfo method = _Service.GetType().GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = (IQueryable<TestModel>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Query_Success_ShouldReturnIQueryriableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            // Act
            var result = (IQueryable<TestModel>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task ReadFilter_Success_ShouldReturnListWithSameLenght_IQueryableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 20 });

            await _Context.SaveChangesAsync();

            // Act
            var result = (IQueryable<TestModel>)method.Invoke(_Service, new object[] { _Context.TestModels });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public void ReadFilter_Success_ShouldReturnListWithSameLenght_EmptyIQueryable()
        {
            // Arrange
            int expCount = 0;
            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = (IQueryable<TestModel>)method.Invoke(_Service, new object[] { _Context.TestModels });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnIEnumerableWithThreeElements()
        {
            // Arrange
            int expCount = 3;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>> [] filter = { e => e.Percent % 10 == 0 };

            // Act
            var result = await (Task<IEnumerable<TestModel>>)method.Invoke(_Service, new object[] { null, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnIEnumerableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>> [] filter = { 
                e => e.Percent % 5 == 0, 
                e => e.Name.Contains("Test4")
            };

            // Act
            var result = await (Task<IEnumerable<TestModel>>)method.Invoke(_Service, new object[] { null, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>> [] filter = null;

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<IEnumerable<TestModel>>) method.Invoke(_Service, new object[] { null, filter })
            );
        }

        [Fact]
        public async Task FindOneWith_Success_ShouldReturnEntityWithPErcentEqualTo10()
        {
            // Arrange
            int percent = 10;
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>> [] filter = { e => e.Percent == percent };

            // Act
            var result = await (Task<TestModel>)method.Invoke(_Service, new object[] { null, filter });

            // Assertion
            Assert.Equal(percent, result.Percent);
        }

        [Fact]
        public async Task FindOneWith_Success_ShouldReturnEntityWithNameEqualToTest4AndPercentEqual25()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>>[] filter = {
                e => e.Name.Contains("Test4"),
                e => e.Percent == 25
            };

            // Act
            var result = await (Task<TestModel>)method.Invoke(_Service, new object[] { null, filter });

            // Assertion
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindOneWith_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>> [] filter = null;

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<TestModel>) method.Invoke(_Service, new object[] { null, filter })
            );
        }

        [Fact]
        public async Task FindOneWith_Failure_ShouldThrowInvalidOperationException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModel, bool>>[] filter = {
                e => e.Name.Contains("Test4")
            };

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => (Task<TestModel>) method.Invoke(_Service, new object[] { null, filter })
            );
        }

        [Fact]
        public async Task Read_Success_ShouldEmptyIEnumerable()
        {
            // Arrange
            int expCount = 0;

            // Act 
            var result = await _Service.Read();

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Read_Success_ShouldIEnumerableWithLargeOf5()
        {
            // Arrange
            int expCount = 5;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act 
            var result = await _Service.Read();

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Read_WithOneParameter_Success_ShouldReturNull()
        {
            // Arrange
            int key = 0;

            // Act 
            var result = await _Service.Read(key);

            // Assertion
            Assert.Null(result);
        }

        [Fact]
        public async Task Read_WithOneParameter_Success_ShouldEntity()
        {
            // Arrange
            int key = 1;

            var tracked = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act 
            var result = await _Service.Read(key);

            // Assertion
            Assert.Equal(tracked.Entity.Name, result.Name);
        }

        [Fact]
        public async Task Paginate_Success_PageOne_ShouldIEnumerableWithLargeOf3()
        {
            // Arrange
            int expCount = 3;
            int number = 3;
            int page = 0;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act 
            var result = await _Service.Paginate(number, page);

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Paginate_Success_PageTwo_ShouldIEnumerableWithLargeOf2()
        {
            // Arrange
            int expCount = 2;
            int number = 3;
            int page = 1;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act 
            var result = await _Service.Paginate(number, page);

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Paginate_Success_PageThree_ShouldEmptyIEnumerable()
        {
            // Arrange
            int expCount = 0;
            int number = 3;
            int page = 2;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act 
            var result = await _Service.Paginate(number, page);

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Paginate_Failure_ShouldThrowArgumentException_NumberBelow1()
        {
            // Arrange
            int number = 0;
            int page = 2;

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _Service.Paginate(number, page)
            );
        }

        [Fact]
        public async Task Paginate_Failure_ShouldThrowArgumentException_PageBelow0()
        {
            // Arrange
            int number = 3;
            int page = -1;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _Service.Paginate(number, page)
            );
        }

        [Fact]
        public async Task CheckExistenceByKey_Success_NotExist()
        {
            // Arrange
            int key = 256;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByKey(key);

            // Assert
            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistenceByKey_Success_Exist()
        {
            // Arrange
            int key = 1;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByKey(key);

            // Assert
            Assert.True(exist);
        }

        [Fact]
        public async Task CheckExistenceByConditions_Failure_ShouldThrowArgumentNullException()
        {
            // Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _Service.CheckExistenceByConditions(null));
        }

        [Fact]
        public async Task CheckExistenceByConditions_Success_NotExist()
        {
            // Arrange
            string name = "Test1";
            int percent = 25;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByConditions(
                e => e.Name == name,
                e => e.Percent == percent
            );

            // Assert
            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistenceByConditions_Success_Exist()
        {
            // Arrange
            string name = "Test3";
            int percent = 20;

            _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Name = "Test2", Percent = 15 });
            _Context.TestModels.Add(new TestModel { Name = "Test3", Percent = 20 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 25 });
            _Context.TestModels.Add(new TestModel { Name = "Test4", Percent = 30 });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByConditions(
                e => e.Name == name,
                e => e.Percent == percent
            );

            // Assert
            Assert.True(exist);
        }
    }
}
