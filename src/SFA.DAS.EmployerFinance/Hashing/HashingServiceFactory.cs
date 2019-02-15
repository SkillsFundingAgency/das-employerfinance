using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Hashing
{
    public class HashingServiceFactory : IHashingServiceFactory
    {
        private readonly HashConfiguration _configuration;

        public HashingServiceFactory(HashConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IHashingService CreateHashingService()
        {
            var hashingService = new HashingService(_configuration.Characters, _configuration.Salt);

            return hashingService;
        }
    }
}