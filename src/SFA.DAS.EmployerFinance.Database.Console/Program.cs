using SFA.DAS.EmployerFinance.Database.Console.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Database.Console
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            using (var container = IoC.Initialize())
            {
                var helper = container.GetInstance<EmployerFinanceDatabaseHelper>();
                var successfulDeployment = helper.Deploy();
                
                return successfulDeployment ? 0 : -1;
            }
        }
    }
}