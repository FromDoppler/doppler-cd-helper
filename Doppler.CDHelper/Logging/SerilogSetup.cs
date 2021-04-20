using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Doppler.CDHelper.Logging
{
    public static class SerilogSetup
    {
        public static LoggerConfiguration SetupSerilog(
            this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            configuration.ConfigureLoggly(hostEnvironment);

            loggerConfiguration
                .WriteTo.Console()
                .Enrich.WithProperty("Application", hostEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
                .Enrich.WithProperty("Platform", Environment.OSVersion.Platform)
                .Enrich.WithProperty("Runtime", Environment.Version)
                .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                .Enrich.FromLogContext();

            if (!hostEnvironment.IsDevelopment())
            {
                loggerConfiguration
                    .WriteTo.Loggly();
            }

            loggerConfiguration.ReadFrom.Configuration(configuration);

            return loggerConfiguration;
        }
    }
}
