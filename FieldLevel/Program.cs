using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace FieldLevel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create and register logger
            Log.Logger = new LoggerConfiguration()
                                .WriteTo.Console()
                                .WriteTo.File("Logs/FieldLevelApi-.txt", rollingInterval: RollingInterval.Day)
                                .CreateLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error booting.");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {                   
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);                    
                });
    }
}
