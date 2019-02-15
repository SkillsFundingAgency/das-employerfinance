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

        public bool TryDecodeValue(string input, out long output)
        {
            var numbers = _hashIds.DecodeLong(input);

            if (!numbers.Any())
            {
                output = default;
                
                return false;
            }

            output = numbers.Single();

            return true;
        }
    }
}