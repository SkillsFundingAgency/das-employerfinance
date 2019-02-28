using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class User : Entity
    {
        public Guid Ref { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public DateTime Created { get; internal set; }
        public DateTime? Updated { get; internal set; }

        public User(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Created = DateTime.UtcNow;
        }

        private User()
        {
        }

        public void Update(string email, string firstName, string lastName)
        {
            if (email != Email || firstName != FirstName || lastName != LastName)
            {
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                Updated = DateTime.UtcNow;
            }
        }
    }
}