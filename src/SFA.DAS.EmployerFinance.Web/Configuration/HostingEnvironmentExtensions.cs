using System;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Web.Configuration
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsPreProduction(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostingEnvironment));
            }
            
            return hostingEnvironment.IsEnvironment(DasEnvironmentName.PreProduction);
        }
    }
}