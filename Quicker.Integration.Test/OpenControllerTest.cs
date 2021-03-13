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
using System.Text;
using System.Threading.Tasks;
using Test.Common.Repository;
using Test.Common.Repository.DTO;
using Xunit;

namespace Quicker.Integration.Test
{
    public class OpenControllerTest : IDisposable
    {
        private readonly TestServer _Server;
        private readonly HttpClient _Client;
        private readonly string _BaseUri;

        public OpenControllerTest()
        {
            _Server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>()
            );

            _Client = _Server.CreateClient();
            _BaseUri = "/api/fakeopen";
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task New_Success_ShouldReturnOK()
        {
            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_BaseUri}/new")
            );
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_Success_ShouldReturnNotFound()
        {
            // Arrange
            var model = new TestModel();

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Post, $"{_BaseUri}")
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(model),
                        Encoding.UTF8,
                        "application/json"
                    )
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Success_WithId_ShouldReturnNotFound()
        {
            // Arrange
            var key = 1;

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, $"{_BaseUri}/{key}")
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Success_WithEntity_ShouldReturnNotFound()
        {
            // Arrange
            var model = new TestModel();

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, $"{_BaseUri}")
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(model),
                        Encoding.UTF8,
                        "application/json"
                    )
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }
    }
}
