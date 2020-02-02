using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace FootballServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG 
            var logger = NLogBuilder.ConfigureNLog("nlog.Development.config").GetCurrentClassLogger();
#else
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
#endif
            try
            {
                logger.Debug("Init Main");

                var host = Host.CreateDefaultBuilder(args)
                               .ConfigureWebHostDefaults(webHostBuilder =>
                               {
                                   webHostBuilder.ConfigureLogging(logging => logging.ClearProviders())
                                                 .UseNLog() 
                                                 .UseKestrel()
                                                 .UseIISIntegration()
                                                 .UseStartup<Startup>()
                                                 .UseUrls("http://*:3143/");
                               })
                               .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
