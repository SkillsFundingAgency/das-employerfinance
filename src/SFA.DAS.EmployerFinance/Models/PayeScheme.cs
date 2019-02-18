using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class PayeScheme
    {
        public string EmployerReferenceNumber { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public DateTime? Deleted { get; private set; }
        
        public PayeScheme(string employerReferenceNumber, string name, DateTime created)
        {
            EmployerReferenceNumber = employerReferenceNumber;
            Name = name;
            Created = created;
        }

        private PayeScheme()
        {
        }
    }
}