using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerFinance.Domain.ExpiredFunds;

namespace SFA.DAS.EmployerFinance.ExpiredFunds
{
    public class ExpiredFunds : IExpiredFunds
    {
        public decimal GetExpiringFundsByDate(Dictionary<CalendarPeriod, decimal> fundsIn, Dictionary<CalendarPeriod, decimal> fundsOut, DateTime date, Dictionary<CalendarPeriod, decimal> expired = null, int expiryPeriod = 24)
        {
            return 0;
        }

        public Dictionary<CalendarPeriod, decimal> GetExpiringFunds(Dictionary<CalendarPeriod, decimal> fundsIn, Dictionary<CalendarPeriod, decimal> fundsOut, Dictionary<CalendarPeriod, decimal> expired = null, int expiryPeriod = 24)
        {
            if (fundsIn == null)
            {
                throw new ArgumentNullException(nameof(fundsIn));
            }

            if (fundsOut == null)
            {
                throw new ArgumentNullException(nameof(fundsOut));
            }

            var expiredFunds = new Dictionary<CalendarPeriod, decimal>();

            CalculateAdjustments(fundsIn, expiryPeriod);

            foreach (var fundsInPair in fundsIn.OrderBy(c => c.Key))
            {
                var expiryDate = new DateTime(fundsInPair.Key.Year, fundsInPair.Key.Month, 1).AddMonths(expiryPeriod);
                var amountDueToExpire = fundsInPair.Value;

                amountDueToExpire = amountDueToExpire > 0
                    ? CalculateExpiryAmount(fundsOut, expiryDate, amountDueToExpire)
                    : 0;

                expiredFunds.Add(new CalendarPeriod(expiryDate.Year, expiryDate.Month), amountDueToExpire);
            }

            return expiredFunds;
        }

        private void CalculateAdjustments(Dictionary<CalendarPeriod, decimal> fundsIn, int expiryPeriod)
        {
            if (fundsIn.Any(c => c.Value < 0))
            {
                var adjustmentsIn = fundsIn.Where(c => c.Value < 0).ToDictionary(key => key.Key, value => value.Value);

                foreach (var adjustment in adjustmentsIn.OrderBy(c => c.Key))
                {
                    var adjustmentStartPeriod = new DateTime(adjustment.Key.Year, adjustment.Key.Month, 1).AddMonths(expiryPeriod * -1);
                    var adjustmentEndPeriod = new DateTime(adjustment.Key.Year, adjustment.Key.Month, 1);
                    var adjustmentAmount = adjustment.Value * -1;

                    foreach (var fundsInValue in fundsIn.Where(c => c.Value > 0).ToDictionary(c => c.Key, c => c.Value)
                        .OrderBy(c => c.Key))
                    {
                        if (new DateTime(fundsInValue.Key.Year, fundsInValue.Key.Month, 1) >= adjustmentStartPeriod &&
                            new DateTime(fundsInValue.Key.Year, fundsInValue.Key.Month, 1) <= adjustmentEndPeriod)
                        {
                            if (fundsInValue.Value >= adjustmentAmount)
                            {
                                fundsIn[fundsInValue.Key] = fundsInValue.Value - adjustmentAmount;
                                break;
                            }

                            if (fundsInValue.Value < adjustmentAmount)
                            {
                                fundsIn[fundsInValue.Key] = 0;
                                adjustmentAmount = adjustmentAmount - fundsInValue.Value;
                            }
                        }
                    }
                }
            }
        }

        private static decimal CalculateExpiryAmount(IDictionary<CalendarPeriod, decimal> fundsOut, DateTime expiryDate, decimal expiryAmount)
        {
            var fundsOutAvailable = fundsOut
                .Where(c => new DateTime(c.Key.Year, c.Key.Month, 1) < expiryDate && c.Value > 0).ToList();

            if (!fundsOutAvailable.Any())
            {
                return expiryAmount;
            }


            foreach (var spentFunds in fundsOutAvailable)
            {
                if (spentFunds.Value >= expiryAmount)
                {
                    fundsOut[spentFunds.Key] = spentFunds.Value - expiryAmount;
                    expiryAmount = 0;
                    break;
                }

                expiryAmount = expiryAmount - spentFunds.Value;
                fundsOut[spentFunds.Key] = 0;
            }

            return expiryAmount;
        }
    }
}