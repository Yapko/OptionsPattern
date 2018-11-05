using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logLevelSwitch =
                new LoggingLevelSwitch(LogEventLevel.Verbose);
            
            var logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.Console(LogEventLevel.Verbose)
                .CreateLogger();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var logMinimumLevelSection = configuration.GetSection("Logging:MinimumLevel");
            ChangeToken.OnChange(logMinimumLevelSection.GetReloadToken, () =>
            {
                logLevelSwitch.MinimumLevel = logMinimumLevelSection.Get<LogEventLevel>();

                Console.WriteLine($"Minimum logging changed to {logLevelSwitch.MinimumLevel}");
            });

            while (true)
            {
                logger.Verbose("Verbose");

                logger.Debug("Debug");

                logger.Information("Information");

                logger.Warning("Warning");

                logger.Error("Error");

                logger.Fatal("Fatal");

                Console.WriteLine($"Minimum logging level is {logLevelSwitch.MinimumLevel}");

                Console.WriteLine("----------------------------------------");
            }
        }
    }
}
