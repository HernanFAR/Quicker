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
    public class FullControllerTest : IDisposable
    {
        private readonly TestServer _Server;
        private readonly HttpClient _Client;
        private readonly string _BaseUri;

        public FullControllerTest()
        {
            _Server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>()
            );

            _Client = _Server.CreateClient();
            _BaseUri = "/api/fakefull";
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Edit_Success_ShouldReturnOK()
        {
            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_BaseUri}/edit")
            );
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_Success_WithParameter_ShouldReturnNotFound()
        {
            // Arrange
            var key = 0;
            var model = new TestModel();

            // Act
            var response = await _Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Put, $"{_BaseUri}/{key}")
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(model),
                        Encoding.UTF8,
                        "application/json"
                    )
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
