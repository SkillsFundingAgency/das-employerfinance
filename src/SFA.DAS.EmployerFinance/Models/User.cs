using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class User : Entity
    {
        public virtual Guid Ref { get; internal set; }
        public virtual string Email { get; internal set; }
        public virtual string FirstName { get; internal set; }
        public virtual string LastName { get; internal set; }
        public virtual DateTime Created { get; internal set; }
        public virtual DateTime? Updated { get; internal set; }

        public User(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Created = DateTime.UtcNow;
        }

        internal User()
        {
        }

        public virtual void Update(string email, string firstName, string lastName)
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