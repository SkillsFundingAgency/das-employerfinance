using System;

namespace SFA.DAS.EmployerFinance.Services
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}