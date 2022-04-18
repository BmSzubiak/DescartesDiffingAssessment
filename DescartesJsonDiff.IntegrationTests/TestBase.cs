using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DescartesJsonDiff.Testing
{
    public class TestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected HttpClient _client;

        public TestBase(WebApplicationFactory<Startup> factory)
        {
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {

            });

            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44335");
        }

    }
}
