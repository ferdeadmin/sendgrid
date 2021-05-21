using DomainModel.OperatingSystem;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Web;

using System;
using System.IO;

namespace Utility.API
{

#pragma warning disable CS1591
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                if (CheckOperatingSystem.IsLinux())
                {
                    NLog.LogManager.Configuration.Variables["mydir"] = "/var/log";
                    BuildWebLinuxHost(args).Build().Run();
                }
                else
                {
                    NLog.LogManager.Configuration.Variables["mydir"] = "c:\\logs";
                    BuildWebWindowHost(args).Build().Run();
                }
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Build Web Window Host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder BuildWebWindowHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory());
                            config.AddJsonFile(
                            "C:/data/config/appsettings.json", optional: true, reloadOnChange: false);
                        }).ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        }).ConfigureWebHost(config =>
                        {
                            config.UseKestrel(options =>
                            {
                                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
                                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                            });
                            config.UseIIS();
                        })
                       .ConfigureLogging(logging =>
                       {
                           logging.ClearProviders();
                           logging.SetMinimumLevel(LogLevel.Trace);
                       })
                       .UseNLog();
        }

        /// <summary>
        /// Build Web Linux Host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder BuildWebLinuxHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory());
                            config.AddJsonFile(
                            "/data/config/appsettings.json", optional: true, reloadOnChange: false);
                        }).ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        }).ConfigureWebHost(config =>
                        {
                            config.UseKestrel(options =>
                            {
                                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(17);
                                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(17);
                            });
                            config.UseIIS();
                        })
                       .ConfigureLogging(logging =>
                       {
                           logging.ClearProviders();
                           logging.SetMinimumLevel(LogLevel.Trace);
                       })
                       .UseNLog();
        }
    }
#pragma warning restore CS1591
}
