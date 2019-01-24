using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public struct HostEnvironmentSettings
    {
        public string EnvironmentName { get; set; }
        public string AppSettingsFilePath { get; set; }
        public LogLevel MinLogLevel { get; set; }
    }
}