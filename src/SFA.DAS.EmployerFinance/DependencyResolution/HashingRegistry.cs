using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Hashing;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class HashingRegistry : Registry
    {
        public HashingRegistry()
        {
            For<IHashingService>().Use(c => GetHashingService(c));
        }

        private IHashingService GetHashingService(IContext context)
        {
            var hashConfiguration = context.GetInstance<HashConfiguration>();
            var hashingService = new HashingService(hashConfiguration.Characters, hashConfiguration.Salt);

            return hashingService;
        }
    }
}