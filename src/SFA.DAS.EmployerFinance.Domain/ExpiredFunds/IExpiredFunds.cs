using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Domain.ExpiredFunds
{
    public interface IExpiredFunds
    {
        decimal GetExpiringFundsByDate(Dictionary<CalendarPeriod, decimal> fundsIn, Dictionary<CalendarPeriod, decimal> fundsOut, DateTime date, Dictionary<CalendarPeriod, decimal> expired=null, int expiryPeriod = 24);
        Dictionary<CalendarPeriod, decimal> GetExpiringFunds(Dictionary<CalendarPeriod, decimal> fundsIn, Dictionary<CalendarPeriod, decimal> fundsOut, Dictionary<CalendarPeriod, decimal> expired = null, int expiryPeriod = 24);
    }
}
