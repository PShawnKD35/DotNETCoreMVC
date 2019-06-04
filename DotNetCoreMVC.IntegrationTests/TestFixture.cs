using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreMVC.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public TestFixture()
        {
            //var dir = Directory.GetCurrentDirectory();
            //var p = "..\\..\\..\\..\\DotNetCoreMVC";
            //var path = Path.GetFullPath(Path.Combine(dir, p));
            try
            {
                var builder = new WebHostBuilder()
                .UseStartup<DotNetCoreMVC.Startup>()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "..\\..\\..\\..\\DotNetCoreMVC"));

                    config.AddJsonFile("appsettings.json");
                });

                _server = new TestServer(builder);

                Client = _server.CreateClient();
                Client.BaseAddress = new Uri("http://localhost:8888");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
