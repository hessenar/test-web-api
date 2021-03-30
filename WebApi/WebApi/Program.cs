using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using WebApi.Middleware;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(lc => lc
                    .WriteTo.Console())
                .WriteTo.Logger(lc => lc
                    .WriteTo.File(@"logs\log-.log",
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day
                    ))
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureServices(services =>
            {
                services.AddHostedService<HandlerService>();
            });
    }
}
