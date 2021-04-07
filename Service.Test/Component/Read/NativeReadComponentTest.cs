using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Component;
using Quicker.Service.Configuration;
using Quicker.Service.Test.Fake.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Repository;
using Xunit;

namespace Quicker.Test.Component.Read
{
    public class NativeReadComponentTest
    {
        private TestContext _Context;

        private ReadComponent<int, Category> _Service;

        #region Query Tests

        [Fact]
        public void Query_Success_ShouldReturnEmptyIQueryriable()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 0;
            MethodInfo method = _Service.GetType().GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);

            // Category
            var result = (IQueryable<Category>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public void Query_Success_ShouldReturnNonEmptyIQueryriable()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(5));

            _Context.SaveChanges();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 5;
            MethodInfo method = _Service.GetType().GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);

            // Category
            var result = (IQueryable<Category>)method.Invoke(_Service, new object[] { });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        #endregion 

        #region ReadFilter tests

        [Fact]
        public void ReadFilter_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assertion
            var invEx = Assert.Throws<TargetInvocationException>(
                () => (IQueryable<Category>)method.Invoke(_Service, new object[] { null })
            );

            var ex = invEx.InnerException as ArgumentNullException;

            Assert.Equal(QuickerExceptionConstants.Entities, ex.ParamName);
        }

        [Fact]
        public void ReadFilter_Success_ShouldReturnListWithSameLenght_EmptyIQueryable()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 0;
            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = (IQueryable<Category>)method.Invoke(_Service, new object[] { _Context.Categories });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task ReadFilter_Success_ShouldReturnListWithSameLenght()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(5));

            _Context.SaveChanges();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 5;
            MethodInfo method = _Service.GetType().GetMethod("ReadFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            await _Context.SaveChangesAsync();

            // Act
            var result = (IQueryable<Category>)method.Invoke(_Service, new object[] { _Context.Categories });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        #endregion

        #region FindManyWithAsync Tests

        [Fact]
        public async Task FindManyWithAsync_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = null;

            // Assertion - Act
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter })
            );

            Assert.Equal(QuickerExceptionConstants.Conditions, ex.ParamName);
        }

        [Fact]
        public async Task FindManyWithAsync_Success_OneCondition_ShouldReturnIEnumerableWithThreeElements()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 2;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { e => e.Id > 1 };

            // Act
            var result = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWithAsync_Success_OneCondition_ShouldReturnEmptyIEnumerable()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 0;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { e => e.Id > 5 };

            // Act
            var result = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWithAsync_Success_TwoConditions_ShouldReturnIEnumerableWithLengthOne()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            int expCount = 1;
            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = {
                e => e.Id > 1,
                e => e.Id < 3
            };

            // Act
            var result = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWithAsync_Failure_ShouldThrowInvalidOperationException_ByPreaction()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = () =>
            {
                return Task.FromResult(false);
            };
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { e => e.Id > 5 };

            // Assertion
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter })
            );

            Assert.Equal(QuickerExceptionConstants.Preaction, ex.Message);
        }

        [Fact]
        public async Task FindManyWithAsync_Success_CustomQuery()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            int expCount = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = () => _Context.Categories.Where(e => e.Id == 2);
            Expression<Func<Category, bool>>[] filter = { e => e.Id > 1 };

            // Act
            var entities = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter });

            Assert.Equal(expCount, entities.Count());
        }

        #endregion

        #region FindOneWithAsync tests
        
        [Fact]
        public async Task FindOneWithAsync_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindManyWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = null;

            // Assertion - Act
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(
                () => (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, query, filter })
            );

            Assert.Equal(QuickerExceptionConstants.Conditions, ex.ParamName);
        }

        [Fact]
        public async Task FindOneWithAsync_Success_OneCondition()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindOneWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { e => e.Id == 1 };

            // Act
            var result = await (Task<Category>)method.Invoke(_Service, new object[] { action, query, filter });

            // Assertion
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindOneWithAsync_Success_TwoCondition_ReturnNull()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindOneWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { 
                e => e.Id > 15,
                e => e.Id == 5 
            };

            // Act
            var result = await (Task<Category>)method.Invoke(_Service, new object[] { action, query, filter });

            // Assertion
            Assert.Null(result);
        }

        [Fact]
        public async Task FindOneWithAsync_Failure_ShouldThrowInvalidOperationException_ByPreaction()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(3));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindOneWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = () =>
            {
                return Task.FromResult(false);
            };
            Func<IQueryable<Category>> query = null;
            Expression<Func<Category, bool>>[] filter = { e => e.Id == 5 };

            // Assertion
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => (Task<Category>)method.Invoke(_Service, new object[] { action, query, filter })
            );

            Assert.Equal(QuickerExceptionConstants.Preaction, ex.Message);
        }

        [Fact]
        public async Task FindOneWithAsync_Success_CustomQuery()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador 
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            MethodInfo method = _Service.GetType().GetMethod("FindOneWithAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = null;
            Func<IQueryable<Category>> query = () => _Context.Categories.Where(e => e.Id > 5);
            Expression<Func<Category, bool>>[] filter = 
            {
                e => e.Id == 15
            };

            // Act
            var entity = await (Task<Category>)method.Invoke(_Service, new object[] { action, query, filter });

            Assert.NotNull(entity);
        }

        #endregion

        #region CheckExistenceAsync WithKey Tests

        [Fact]
        public async Task CheckExistenceAsync_WithKey_Success_ReturnTrue()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 5;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = null;

            // Act
            var exist = await _Service.CheckExistenceAsync(expId, action);

            Assert.True(exist);
        }

        [Fact]
        public async Task CheckExistenceAsync_WithKey_Success_ReturnFalse()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 26;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = null;

            // Act
            var exist = await _Service.CheckExistenceAsync(expId, action);

            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistenceAsync_WithKey_Failure_ShouldThrowInvalidOperationException_ByPreAction()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 26;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = () => Task.FromResult(false);

            // Act
            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(
                () => _Service.CheckExistenceAsync(expId, action)
            );

            Assert.Equal(QuickerExceptionConstants.Preaction, ex.Message);
        }

        #endregion

        #region CheckExistenceAsync WithConditions Tests

        [Fact]
        public async Task CheckExistenceAsync_WithConditions_Success_ReturnTrue()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 1;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = null;
            Expression<Func<Category, bool>>[] conditions = {
                e => e.Id == expId
            };

            // Act
            var exist = await _Service.CheckExistenceAsync(action, conditions);

            Assert.True(exist);
        }

        [Fact]
        public async Task CheckExistenceAsync_WithConditions_Success_ReturnFalse()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 26;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = null;
            Expression<Func<Category, bool>>[] conditions = {
                e => e.Id == expId
            };

            // Act
            var exist = await _Service.CheckExistenceAsync(action, conditions);

            Assert.False(exist);
        }

        [Fact]
        public async Task CheckExistenceAsync_WithConditions_Failure_ShouldThrowArgumentNullException()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = null;
            Expression<Func<Category, bool>>[] conditions = null;

            // Act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(
                () => _Service.CheckExistenceAsync(action, conditions)
            );

            Assert.Equal(QuickerExceptionConstants.Conditions, ex.ParamName);
        }

        [Fact]
        public async Task CheckExistenceAsync_WithConditions_Failure_ShouldThrowInvalidOperationException_ByPreAction()
        {
            // Arrange
            _Context = new ConnectionFactory().CreateContextForSQLite();

            // Generador
            int expId = 26;
            int id = 1;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            _Context.Categories.AddRange(faker.Generate(25));

            await _Context.SaveChangesAsync();

            var container = new ServiceCollection()
                .AddLogging();

            container.AddQuickerConfiguration();
            container.AddScoped<DbContext, TestContext>(e => _Context);

            _Service = new FakeNativeReadComponent(container.BuildServiceProvider());

            Func<Task<bool>> action = () => Task.FromResult(false);
            Expression<Func<Category, bool>>[] conditions = {
                e => e.Id == expId
            };

            // Act
            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(
                () => _Service.CheckExistenceAsync(expId, action)
            );

            Assert.Equal(QuickerExceptionConstants.Preaction, ex.Message);
        }

        #endregion
    }
}
