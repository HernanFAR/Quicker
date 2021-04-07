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

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnIEnumerableWithThreeElements()
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
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            Expression<Func<Category, bool>>[] filter = { e => e.Id > 1 };

            // Act
            var result = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { null, null, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnEmptyIEnumerable()
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
            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            Expression<Func<Category, bool>>[] filter = { e => e.Id > 5 };

            // Act
            var result = await (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { null, null, filter });

            // Assertion
            Assert.Equal(expCount, result.Count());
        }

        [Fact]
        public async Task FindManyWith_Success_ShouldReturnDefaultBecauseAction()
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

            MethodInfo method = _Service.GetType().GetMethod("FindManyWith", BindingFlags.NonPublic | BindingFlags.Instance);

            Func<Task<bool>> action = () =>
            {
                return Task.FromResult(false);
            };

            Expression<Func<Category, bool>>[] filter = { e => e.Id > 5 };

            // Assertion
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => (Task<IEnumerable<Category>>)method.Invoke(_Service, new object[] { action, null, filter })
            );

            Assert.Equal(QuickerExceptionConstants.Preaction, ex.Message);
        }
    }
}
