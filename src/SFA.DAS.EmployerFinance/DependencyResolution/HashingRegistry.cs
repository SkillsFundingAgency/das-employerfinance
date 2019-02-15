using SFA.DAS.EmployerFinance.Hashing;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class HashingRegistry : Registry
    {
        public HashingRegistry()
        {
            For<IHashingService>().Use(c => c.GetInstance<IHashingServiceFactory>().CreateHashingService()).Singleton();
            For<IHashingServiceFactory>().Use<HashingServiceFactory>();
        }
    }
}