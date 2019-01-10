using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static async Task Main()
        {
            using (var container = IoC.Initialize())
            {
            }
        }
    }
}
