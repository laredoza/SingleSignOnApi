// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.AdminApi
{
    #region Usings

    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;
    using NLog.Web;

    #endregion

    namespace DotnetTradingBot.Api
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                // NLog: setup the logger first to catch all errors
                var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Development.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();

                LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

                var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
                try
                {
                    logger.Debug("Init main");
                    CreateWebHostBuilder(args).Build().Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"${ex.InnerException}");
                    Console.WriteLine($"${ex}");
                    logger.Error(ex, "Stopped program because of exception");
                }
                finally
                {
                    LogManager.Shutdown();
                }
            }

            public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog()
                    .UseStartup<Startup>();
        }
    }
}