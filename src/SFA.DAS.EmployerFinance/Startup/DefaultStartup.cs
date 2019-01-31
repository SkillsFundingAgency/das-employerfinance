using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Startup
{
    public class DefaultStartup : IRunAtStartup
    {
        private readonly IEnumerable<IRunAtStartup> _startups;

        public DefaultStartup(IEnumerable<IRunAtStartup> startups)
        {
            _startups = startups;
        }

        public Task StartAsync()
        {
            return Task.WhenAll(_startups.Select(t => t.StartAsync()));
        }

        public Task StopAsync()
        {
            return Task.WhenAll(_startups.Reverse().Select(t => t.StopAsync()));
        }
    }
}
