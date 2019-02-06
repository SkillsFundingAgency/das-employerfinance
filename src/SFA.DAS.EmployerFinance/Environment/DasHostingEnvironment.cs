using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Hosting.Internal;

namespace SFA.DAS.EmployerFinance.Environment
{
    public static class DasHostingEnvironment
    {
        /// <summary>
        /// Creates a HostingEnvironment (useful for example, in console apps when not building a host).
        /// There are two different HostingEnvironment's in the framework. We've standardised on the AspNetCore version for consistency (for now).
        /// For more details, see https://andrewlock.net/the-asp-net-core-generic-host-namespace-clashes-and-extension-methods/
        /// </summary>
        public static IHostingEnvironment Create(string environmentName)
        {
            var dasEnvNameToCoreEnvNameMap = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"LOCAL", EnvironmentName.Development },
                {"AT", DasEnvironmentName.AcceptanceTest },
                {"TEST", DasEnvironmentName.Test },
                {"TEST2", DasEnvironmentName.Test2 },
                {"PREPROD", DasEnvironmentName.PreProduction },
                {"PROD", EnvironmentName.Production },
                {"MO", DasEnvironmentName.ModelOffice },
                {"DEMO", DasEnvironmentName.Demonstration }
            };
            
            var coreEnvName = dasEnvNameToCoreEnvNameMap[environmentName] ?? environmentName;
            
            return new HostingEnvironment
            {
                EnvironmentName = coreEnvName,
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
                //ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory)
            };
        }
    }
}