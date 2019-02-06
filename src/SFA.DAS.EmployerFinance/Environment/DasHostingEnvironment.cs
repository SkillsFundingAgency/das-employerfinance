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
        public static IHostingEnvironment Create(string environmentName)
        {
            var dasEnvNameToCoreEnvNameMap = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"LOCAL", EnvironmentName.Development },
                {"PREPROD", DasEnvironmentName.PreProduction },
                {"PROD", EnvironmentName.Production }
            };

            //todo: add extensions to IHostingEnvironment for IsTest, IsTest2 etc.
            var coreEnvName = dasEnvNameToCoreEnvNameMap[environmentName] ?? environmentName;
            
            //todo: can we get from the generic host HostBuilder?
            //todo: the concrete and interface use the generic versions in Microsoft.Extensions.Hosting, but we want seamless interoperability with the mvc version. should we use that version instead?
            // see https://andrewlock.net/the-asp-net-core-generic-host-namespace-clashes-and-extension-methods/
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