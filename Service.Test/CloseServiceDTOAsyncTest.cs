using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Mapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Services.Test
{
    public class CloseServiceDTOAsyncTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly IMapper _Mapper;
        private readonly CloseServiceAsync<int, TestModelRelation, TestModelRelationDTO> _Service;

        public CloseServiceDTOAsyncTest() 
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();
            _Mapper = new MapperConfiguration(config => {
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

            _Service = new Fake.FakeCloseServiceDTO(container.BuildServiceProvider());
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
            var result = (IQueryable<TestModelRelation>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task Query_Success_ShouldReturnIQueryriableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var result = (IQueryable<TestModelRelation>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task ReadFilter_Success_ShouldReturnListWithSameLenght_IQueryableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var result = (IQueryable<TestModelRelation>)method.Invoke(_Service, new object[] { _Context.TestModelRelations });

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
            var result = (IQueryable<TestModelRelation>)method.Invoke(_Service, new object[] { _Context.TestModelRelations });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnIEnumerableWithTwoElements()
        {
            // Arrange
            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>> [] filter = { e => e.Name == "TestRelation4" };

            // Act
            var result = await (Task<IEnumerable<TestModelRelationDTO>>)method.Invoke(_Service, new object[] { filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnIEnumerableWithFiveElements()
        {
            // Arrange
            int expCount = 5;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Id = 2, Name = "Test2", Percent = 20 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation5", TestModelId = 2 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>> [] filter = { 
                e => e.TestModelId == 1, 
                e => e.Name.Contains("TestRelation")
            };

            // Act
            var result = await (Task<IEnumerable<TestModelRelationDTO>>)method.Invoke(_Service, new object[] { filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Id = 2, Name = "Test2", Percent = 20 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation5", TestModelId = 2 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>> [] filter = null;

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<IEnumerable<TestModelRelationDTO>>) method.Invoke(_Service, new object[] { filter })
            );
        }

        [Fact]
        public async Task FindOneWith_Success_ShouldReturnEntityWithPErcentEqualTo10()
        {
            // Arrange
            string name = "TestRelation5";
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Id = 2, Name = "Test2", Percent = 20 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation5", TestModelId = 2 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>> [] filter = { 
                e => e.TestModelNavigation.Percent == 20
            };

            // Act
            var result = await (Task<TestModelRelationDTO>)method.Invoke(_Service, new object[] { filter });

            // Assertion
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task FindOneWith_Success_ShouldReturnEntityWithNameEqualToTest4AndPercentEqual25()
        {
            // Arrange
            string expName = "TestRelation3";
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModels.Add(new TestModel { Id = 2, Name = "Test2", Percent = 20 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "TestRelation5", TestModelId = 2 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>>[] filter = {
                e => e.Name.Contains("TestRelation3"),
                e => e.TestModelNavigation.Percent == 10
            };

            // Act
            var result = await (Task<TestModelRelationDTO>)method.Invoke(_Service, new object[] { filter });

            // Assertion
            Assert.Equal(expName, result.Name);
        }

        [Fact]
        public async Task FindOneWith_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            Expression<Func<TestModelRelation, bool>> [] filter = null;

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<TestModelRelationDTO>) method.Invoke(_Service, new object[] { filter })
            );
        }

        [Fact]
        public async Task FindOneWith_Failure_ShouldThrowInvalidOperationException()
        {
            // Arrange
            MethodInfo method = _Service.GetType().GetMethod("FindOneWith", BindingFlags.NonPublic | BindingFlags.Instance);

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            Expression<Func<TestModelRelation, bool>>[] filter = {
                e => e.Name.Contains("Test4")
            };

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => (Task<TestModelRelationDTO>) method.Invoke(_Service, new object[] { filter })
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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            var tracked = _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

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

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Test1", Percent = 10 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test4", TestModelId = 1 });

            await _Context.SaveChangesAsync();

            // Act / Assertion
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _Service.Paginate(number, page)
            );
        }

        [Fact]
        public async Task CheckExistence_Success_NotExist()
        {
            // Arrange
            int key = 256;

            var tracked = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });

            await _Context.SaveChangesAsync();


            // Act
            var exist = await _Service.CheckExistenceByKey(key);

            // Assert
            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistence_Success_Exist()
        {
            // Arrange
            int key = 1;

            var tracked = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });

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

            var tracked = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            var tracked2da = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = tracked2da.Entity.Id });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByConditions(
                e => e.Name == name,
                e => e.TestModelId == 3
            );

            // Assert
            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistenceByConditions_Success_Exist()
        {
            // Arrange
            string name = "Test2";

            var tracked = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });
            var tracked2da = _Context.TestModels.Add(new TestModel { Name = "Test1", Percent = 10 });

            await _Context.SaveChangesAsync();

            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test1", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test2", TestModelId = tracked.Entity.Id });
            _Context.TestModelRelations.Add(new TestModelRelation { Name = "Test3", TestModelId = tracked2da.Entity.Id });

            await _Context.SaveChangesAsync();

            // Act
            var exist = await _Service.CheckExistenceByConditions(
                e => e.Name == name,
                e => e.TestModelId == tracked.Entity.Id
            );

            // Assert
            Assert.True(exist);
        }
    }
}
