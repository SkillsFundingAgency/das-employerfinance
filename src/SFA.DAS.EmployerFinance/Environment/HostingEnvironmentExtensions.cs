using System;
using Microsoft.AspNetCore.Hosting;

namespace SFA.DAS.EmployerFinance.Environment
{
  /// <summary>
  /// Extension methods for <see cref="T:Microsoft.AspNetCore.Hosting.IHostingEnvironment" />.
  /// </summary>
  public static class HostingEnvironmentExtensions
  {
    /// <summary>
    /// Checks if the current hosting environment name is <see cref="F:SFA.DAS.EmployerFinance.Environment.DasEnvironmentName.PreProduction" />.
    /// </summary>
    /// <param name="hostingEnvironment">An instance of <see cref="T:Microsoft.AspNetCore.Hosting.IHostingEnvironment" />.</param>
    /// <returns>True if the environment name is <see cref="F:SFA.DAS.EmployerFinance.Environment.DasEnvironmentName.PreProduction" />, otherwise false.</returns>
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