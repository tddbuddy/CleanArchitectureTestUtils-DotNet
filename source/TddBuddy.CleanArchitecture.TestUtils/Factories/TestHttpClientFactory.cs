using System;
using System.Net.Http;
using Microsoft.Owin.Testing;

namespace TddBuddy.CleanArchitecture.TestUtils.Factories
{
    public static class TestHttpClientFactory
    {
        public static HttpClient CreateClient(TestServer server)
        {
            var client = new HttpClient(server.Handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            return client;
        }
    }
}
