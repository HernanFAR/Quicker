using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Integration.Test
{
    public class CloseControllerTest : IDisposable
    {
        private readonly TestServer _Server;
        private readonly HttpClient _Client;
        private readonly string _BaseUri;

        public CloseControllerTest()
        {
            _Server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>()
            );

            _Client = _Server.CreateClient();
            _BaseUri = "/api/fakeclose";
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Read_Success_ShouldReturnNoContent()
        {
            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, _BaseUri)
            );
            
            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Read_Success_WithParameter_ShouldReturnNotFound()
        {
            // Arrange
            int key = 1;

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_BaseUri}/{key}")
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Paginate_Success_ShouldReturnNoContent()
        {
            // Arrange
            int page = 0;
            int number = 10;

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_BaseUri}/paginate?page={page}&number={number}")
            );

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task CheckExistenceByKey_Success_ShouldReturnFalse()
        {
            // Arrange
            int key = 0;

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_BaseUri}/exists/{key}")
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
