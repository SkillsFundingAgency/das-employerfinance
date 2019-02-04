using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Startup
{
    public interface IRunAtStartup
    {
        Task StartAsync();
        Task StopAsync();
    }
}
