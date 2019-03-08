using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class Provider : Entity
    {
        public virtual long Ukprn { get; private set; }
        public virtual string Name { get; private set; }
        public virtual DateTime Created { get; private set; }
        public virtual DateTime? Updated { get; private set; }

        private Provider()
        {
        }
    }
}