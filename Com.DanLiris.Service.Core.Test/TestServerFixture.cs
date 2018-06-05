using Com.DanLiris.Service.Core.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace Com.DanLiris.Service.Core.Test
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public TestServerFixture()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            
            var builder = new WebHostBuilder().UseConfiguration(configuration).UseStartup<Startup>();

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }

    [CollectionDefinition("TestFixture Collection")]
    public class TestFixtureCollection : ICollectionFixture<TestServerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
