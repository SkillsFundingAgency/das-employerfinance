using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public class GoogleAnalyticsConfigurationFactory : IGoogleAnalyticsConfigurationFactory
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoogleAnalyticsConfigurationFactory(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public GoogleAnalyticsConfiguration CreateConfiguration()
        {
            var configuration = new GoogleAnalyticsConfiguration();

            if (_hostingEnvironment.IsPreProduction())
            {
                configuration.ContainerId = "GTM-KWQBWGJ";
                configuration.TrackingId = "UA-83918739-9";
            }
            else if (_hostingEnvironment.IsProduction())
            {
                configuration.ContainerId = "GTM-KWQBWGJ";
                configuration.TrackingId = "UA-83918739-9";
            }

            return configuration;
        }
    }
}