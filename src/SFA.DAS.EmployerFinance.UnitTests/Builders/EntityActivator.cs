using System;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.UnitTests.Builders
{
    public static class EntityActivator
    {
        public static T CreateInstance<T>() where T : Entity
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}