using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quicker.Abstracts.Service;
using Quicker.Test.Mapper;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Quicker.Test.Services
{
    public class CloseServiceDTOAsyncTest : IDisposable
    {
        private TestContext _Context;
        private IMapper _Mapper;
        private CloseServiceAsync<int, TestModelRelation, TestModelRelationDTO> _Service;

        public CloseServiceDTOAsyncTest() 
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();
            _Mapper = new MapperConfiguration(config => {
                config.AddProfile<TestModelRelationMapper>();
            })
            .CreateMapper();
            _Service = new FakeServices.FakeCloseServiceDTO(_Context, _Mapper);
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
    }
}
