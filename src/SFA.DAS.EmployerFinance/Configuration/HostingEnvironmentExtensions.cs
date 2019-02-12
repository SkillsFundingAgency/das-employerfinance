using System;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerFinance.Configuration
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