using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quicker.Abstracts.Controller;
using Quicker.Controller.Test.Fake;
using Quicker.Services.Test.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Mapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Controller.Test
{
    public class FullControllerTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly FullControllerAsync<int, TestModel, FakeFullService> _Controller;

        public FullControllerTest()
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

            var service = new FakeFullService(container.BuildServiceProvider());

            _Controller = new FakeFullController(service);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Update_Success_ShouldReturnOk()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModel
            {
                Id = 1,
                Name = "Name2",
                Percent = 2
            };

            // Act
            var response = await _Controller.Update(model.Id, model);

            // Assert
            Assert.True(response.Result is OkObjectResult);

            var edited = (TestModel) (response.Result as OkObjectResult).Value;

            Assert.Equal(model.Name, edited.Name);
            Assert.Equal(model.Percent, edited.Percent);
        }

        [Fact]
        public async Task Update_Success_ShouldReturnBadRequest()
        {
            // Arrange
            // Act
            var response = await _Controller.Update(0, null);

            // Assert
            Assert.True(response.Result is BadRequestResult);
        }

        [Fact]
        public async Task Update_Success_ShouldReturnNotFound()
        {
            // Arrange
            var model = new TestModel
            {
                Name = "Name2",
                Percent = 2
            };

            // Act
            var response = await _Controller.Update(0, model);

            // Assert
            Assert.True(response.Result is NotFoundResult);
        }

        [Fact]
        public async Task Update_Success_ShouldReturnNotAcceptable()
        {
            // Arrange
            var expStatusCode = 406;
            var model = new TestModel
            {
                Id = 1,
                Name = "Name2",
                Percent = 2
            };

            // Act
            var response = await _Controller.Update(0, model);

            // Assert
            Assert.True(response.Result is StatusCodeResult);
            Assert.Equal(expStatusCode, (response.Result as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task Update_Success_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModel
            {
                Id = 1,
                Name = "",
                Percent = 2
            };

            // Act
            var response = await _Controller.Update(model.Id, model);

            // Assert
            Assert.True(response.Result is UnprocessableEntityObjectResult);
        }

        [Fact]
        public void Edit_Success_ShouldReturnOk()
        {
            // Act
            var response = _Controller.New();
            var validTypes = new List<string> { 
                typeof(string).Name, 
                typeof(char).Name, 
                typeof(sbyte).Name, 
                typeof(byte).Name, 
                typeof(short).Name,
                typeof(ushort).Name,
                typeof(int).Name, 
                typeof(uint).Name,
                typeof(long).Name,
                typeof(ulong).Name,
                typeof(float).Name,
                typeof(float).Name,
                typeof(double).Name,
                typeof(decimal).Name,
                typeof(DateTime).Name,
                typeof(DateTimeOffset).Name,
                typeof(bool).Name
            };

            // Assert 
            Assert.True(response.Result is OkObjectResult);

            var list = (Dictionary<string, string>) (response.Result as OkObjectResult).Value;

            foreach(var keyValue in list)
                Assert.Contains(keyValue.Value, validTypes);

        }
    }
}
