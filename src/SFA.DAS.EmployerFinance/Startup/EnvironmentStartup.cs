using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class EnvironmentStartup
    {
        public static IHostBuilder UseDasEnvironment(this IHostBuilder hostBuilder)
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableName.EnvironmentName);
            var mappedEnvironmentName = DasEnvironmentName.Map[environmentName];
            
            return hostBuilder.UseEnvironment(mappedEnvironmentName);
        }
        
        public static IWebHostBuilder UseDasEnvironment(this IWebHostBuilder hostBuilder)
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableName.EnvironmentName);
            var mappedEnvironmentName = DasEnvironmentName.Map[environmentName];
            
            return hostBuilder.UseEnvironment(mappedEnvironmentName);
        }
    }
}