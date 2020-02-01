using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.TestHost;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using FoorballServices.SqlDataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FootballServices.WebAPI.Tests.Unit
{
    public class FootballWebApi
    {
        public async Task<IHost> GetServer()
        {
            var dbName = Guid.NewGuid();

            var path = Assembly.GetAssembly(typeof(FootballWebApi)).Location;
            var hostBuilder = new HostBuilder()
                        .ConfigureWebHost(webHost =>
                        {
                            webHost.UseTestServer();
                            webHost.UseStartup<Startup>();
                            webHost.UseEnvironment("Test");
                        });


            return await hostBuilder.StartAsync();
        }
    }
}
