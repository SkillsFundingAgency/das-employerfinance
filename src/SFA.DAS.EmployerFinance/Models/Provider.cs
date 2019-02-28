using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class Provider : Entity
    {
        public long Ukprn { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }

        private Provider()
        {
        }
    }
}