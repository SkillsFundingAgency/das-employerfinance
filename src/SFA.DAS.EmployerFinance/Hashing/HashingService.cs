using System.Linq;
using HashidsNet;

namespace SFA.DAS.EmployerFinance.Hashing
{
    public class HashingService : IHashingService
    {
        private readonly Hashids _hashIds;

        public HashingService(string characters, string salt)
        {
            _hashIds = new Hashids(salt, 6, characters);
        }

        public bool TryDecodeLong(string input, out long output)
        {
            var results = _hashIds.DecodeLong(input);
            var hasResults = results.Any();

            output = hasResults ? results.Single() : default;
            
            return hasResults;
        }
    }
}