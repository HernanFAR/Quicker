using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Controller;
using Quicker.Controller.Test.Fake;
using Quicker.Services.Test.Fake;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Mapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Controller.Test
{
    public class FakeCloseControllerTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly CloseControllerAsync<int, TestModel, FakeCloseService> _Controller;

        public FakeCloseControllerTest()
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

            var service = new FakeCloseService(container.BuildServiceProvider());

            _Controller = new FakeCloseController(service);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Read_Success_ShouldReturnOK_WithZeroEntities()
        {
            // Arrange
            int expCount = 0;

            // Act
            var response = await _Controller.Read();

            // Assert
            Assert.Equal(expCount, response.Value.ToList().Count);
        }
    }
}
