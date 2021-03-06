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
    public class OpenControllerDTOTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly OpenControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeOpenServiceDTO> _Controller;
        private readonly IMapper _Mapper;

        public OpenControllerDTOTest()
        {
            _Context = new ConnectionFactory().CreateContextForSQLite();

            var container = new ServiceCollection()
                .AddLogging();
            _Mapper = new MapperConfiguration(config =>
            {
                config.AddProfile<TestModelRelationMapper>();
            })
            .CreateMapper();

            container.AddQuickerConfiguration(config =>
            {
                config.UseLogger = true;
                config.UseAutoMapper = true;
            });
            container.AddScoped<DbContext, TestContext>(e => _Context);
            container.AddScoped(e => _Mapper);

            var service = new FakeOpenServiceDTO(container.BuildServiceProvider());

            _Controller = new FakeOpenControllerDTO(service);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Create_Success_ShouldReturnCreated()
        {
            // Arrange
            int expCount = 1;
            int expStatusCode = 201;
            _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Name = "Name2",
                TestModelId = 1
            };

            // Act
            var response = await _Controller.Create(model);

            // Assert
            Assert.True(response.Result is ObjectResult);
            Assert.Equal(expStatusCode, (response.Result as ObjectResult).StatusCode);
            Assert.Equal(expCount, await _Context.TestModels.CountAsync());
        }

        [Fact]
        public async Task Create_Success_ShouldReturnBadRequest()
        {
            // Arrange
            int expCount = 0;
            int expStatusCode = 400;

            // Act
            var response = await _Controller.Create(null);

            // Assert
            Assert.True(response.Result is BadRequestResult);
            Assert.Equal(expStatusCode, (response.Result as StatusCodeResult).StatusCode);
            Assert.Equal(expCount, await _Context.TestModels.CountAsync());
        }

        [Fact]
        public async Task Create_Success_ShouldReturnConflict()
        {
            // Arrange
            int expCount = 1;
            int expStatusCode = 409;
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "Name", TestModelNavigation = entry.Entity });

            await _Context.SaveChangesAsync();

            var model = new TestModelRelationDTO
            {
                Id = 1,
                Name = "Name2",
                TestModelId = 1
            };

            // Act
            var response = await _Controller.Create(model);

            // Assert
            Assert.True(response.Result is ConflictObjectResult);
            Assert.Equal(expStatusCode, (response.Result as ObjectResult).StatusCode.Value);
            Assert.Equal(expCount, await _Context.TestModels.CountAsync());
        }

        [Fact]
        public async Task Create_Success_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            int expCount = 0;
            int expStatusCode = 422;

            var model = new TestModelRelationDTO
            {
                Id = 1,
                Name = "",
                TestModelId = 2
            };

            // Act
            var response = await _Controller.Create(model);

            // Assert
            Assert.True(response.Result is UnprocessableEntityObjectResult);
            Assert.Equal(expStatusCode, (response.Result as ObjectResult).StatusCode.Value);
            Assert.Equal(expCount, await _Context.TestModels.CountAsync());
        }

        [Fact]
        public void New_Success_ShouldReturnOk()
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

        [Fact]
        public async Task Delete_Entity_Success_ShouldReturnOk()
        {
            // Arrange
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "Name", TestModelNavigation = entry.Entity });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Delete(new TestModelRelationDTO { Id = 1 });

            // Assert 
            Assert.True(response is OkResult);
        }

        [Fact]
        public async Task Delete_Entity_Success_ShouldReturnBadRequest()
        {
            // Act
            var response = await _Controller.Delete(null);

            // Assert 
            Assert.True(response is BadRequestResult);
        }

        [Fact]
        public async Task Delete_Entity_Success_ShouldReturnNotAccepted()
        {
            // Arrange
            int expStatus = 406;
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "Name", TestModelNavigation = entry.Entity });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Delete(new TestModelRelationDTO { Id = 0 });

            // Assert 
            Assert.True(response is StatusCodeResult);
            Assert.Equal(expStatus, (response as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task Delete_Key_Success_ShouldReturnOk()
        {
            // Arrange
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "Name", TestModelNavigation = entry.Entity });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Delete(1);

            // Assert 
            Assert.True(response is OkResult);
        }

        [Fact]
        public async Task Delete_Key_Success_ShouldReturnNotAccepted()
        {
            // Arrange
            int expStatus = 406;
            var entry = _Context.TestModels.Add(new TestModel { Id = 1, Name = "Name", Percent = 1 });
            _Context.TestModelRelations.Add(new TestModelRelation { Id = 1, Name = "Name", TestModelNavigation = entry.Entity });

            await _Context.SaveChangesAsync();

            // Act
            var response = await _Controller.Delete(0);

            // Assert 
            Assert.True(response is StatusCodeResult);
            Assert.Equal(expStatus, (response as StatusCodeResult).StatusCode);
        }
    }
}
