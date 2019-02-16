using SFA.DAS.EmployerFinance.Data;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IEmployerFinanceDbContextFactory>().Use<EmployerFinanceDbContextWithNewTransactionFactory>();
        }
    }
}