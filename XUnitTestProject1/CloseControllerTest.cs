﻿using AutoMapper;
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
    public class CloseControllerTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly CloseControllerAsync<int, TestModel, FakeCloseService> _Controller;

        public CloseControllerTest()
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
        public async Task Read_Success_ShouldReturnNoContent_WithZeroEntities()
        {
            // Act
            var response = await _Controller.Read();

            // Assert
            Assert.True(response.Result is NoContentResult);
        }

        [Fact]
        public async Task Read_Success_ShouldReturnOK_WithOneEntities()
        {
            // Arrange
            int expCount = 1;
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Read();

            // Assert
            Assert.True(response.Result is OkObjectResult);

            var list = (IEnumerable<TestModel>) (response.Result as OkObjectResult).Value;

            Assert.Equal(expCount, list.ToList().Count);
        }

        [Fact]
        public async Task Read_Success_WithParameter_ShouldReturnNotFound()
        {
            // Arrange
            int key = 1;

            // Act
            var response = await _Controller.Read(key);

            // Assert
            Assert.True(response.Result is NotFoundResult);
        }

        [Fact]
        public async Task Read_Success_WithParameter_ShouldReturnOk()
        {
            // Arrange
            int key = 1;
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Read(key);

            var okObjectResult = (OkObjectResult)response.Result;
            var entity = (TestModel)okObjectResult.Value;

            // Assert
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Paginate_Success_ShouldReturnOk()
        {
            // Arrange
            int page = 0;
            int number = 10;
            int expCount = 1;

            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Paginate(number, page);

            // Assert 
            Assert.True(response.Result is OkObjectResult);

            var list = (IEnumerable<TestModel>) (response.Result as OkObjectResult).Value;

            Assert.Equal(expCount, list.ToList().Count);
        }

        [Fact]
        public async Task Paginate_Success_ShouldReturnNoContent()
        {
            // Arrange
            int page = 0;
            int number = 10;

            // Act
            var response = await _Controller.Paginate(number, page);

            // Assert 
            Assert.True(response.Result is NoContentResult);
        }

        [Fact]
        public async Task Paginate_Success_ShouldReturnUnProcessableEntity_PageUnderZero()
        {
            // Arrange
            int page = -1;
            int number = 10;

            // Act
            var response = await _Controller.Paginate(number, page);

            // Assert 
            Assert.True(response.Result is UnprocessableEntityObjectResult);
        }

        [Fact]
        public async Task Paginate_Success_ShouldReturnUnProcessableEntity_NumberUnderOne()
        {
            // Arrange
            int page = 0;
            int number = 0;

            // Act
            var response = await _Controller.Paginate(number, page);

            // Assert 
            Assert.True(response.Result is UnprocessableEntityObjectResult);
        }

        [Fact]
        public async Task CheckExistenceByKey_Success_ShouldReturnTrue()
        {
            // Arrange
            int key = 1;
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.CheckExistenceByKey(key);

            var okObjectResult = (OkObjectResult)response.Result;
            var exist = (bool)okObjectResult.Value;

            // Assert
            Assert.True(exist);
        }

        [Fact]
        public async Task CheckExistenceByKey_Success_ShouldReturnFalse()
        {
            // Arrange
            int key = 0;
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.CheckExistenceByKey(key);

            var okObjectResult = (OkObjectResult)response.Result;
            var exist = (bool)okObjectResult.Value;

            // Assert
            Assert.False(exist);
        }
    }
}
