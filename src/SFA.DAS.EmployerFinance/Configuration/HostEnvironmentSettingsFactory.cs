using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public static class HostEnvironmentSettingsFactory
    {
        public static HostEnvironmentSettings Create(IEnvironmentService environmentService)
        {
            var settings = new HostEnvironmentSettings
            {
                EnvironmentName = EnvironmentName.Production, 
                AppSettingsFilePath = "appsettings.json"
            };
            
            var loglevelStr = environmentService.GetVariable(AppSettingsKeys.LogLevel);
            settings.MinLogLevel = Enum.TryParse<LogLevel>(loglevelStr, out var logLevel) ? logLevel : LogLevel.Debug;

            if (!environmentService.IsCurrent(DasEnv.LOCAL))
            {
                return settings;
            }
            
            settings.EnvironmentName = EnvironmentName.Development;
            settings.AppSettingsFilePath = "appsettings.Development.json";

            return settings;
        }
    }
}