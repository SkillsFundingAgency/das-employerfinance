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
            var expiredFunds = GetExpiringFunds(fundsIn, fundsOut, expired, expiryPeriod);

            if (expiredFunds.Any())
            {
                var expiredFundsKey = expiredFunds.Keys.SingleOrDefault(key => key.Year.Equals(date.Year) && key.Month.Equals(date.Month));
                if (expiredFundsKey != null)
                {
                    return expiredFunds[expiredFundsKey];
                }
            }

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

            CalculateAndApplyAdjustments(fundsIn, expiryPeriod);

            CalculateAndApplyExpiredFundsToFundsOut(fundsOut, expired, expiryPeriod);
            
            foreach (var fundsInPair in fundsIn.OrderBy(c => c.Key))
            {
                var expiryDate = new DateTime(fundsInPair.Key.Year, fundsInPair.Key.Month, 1).AddMonths(expiryPeriod);
                var amountDueToExpire = fundsInPair.Value;


                var alreadyExpiredAmount = expired?.Keys.FirstOrDefault(c => c.Year.Equals(expiryDate.Year)
                                                                         && c.Month.Equals(expiryDate.Month));

                if (alreadyExpiredAmount != null)
                {
                    amountDueToExpire = expired[alreadyExpiredAmount];
                }
                else
                {
                    amountDueToExpire = amountDueToExpire > 0
                        ? CalculateExpiryAmount(fundsOut, expiryDate, amountDueToExpire)
                        : 0;
                }
                

                expiredFunds.Add(new CalendarPeriod(expiryDate.Year, expiryDate.Month), amountDueToExpire);
            }

            return expiredFunds;
        }

        private void CalculateAndApplyExpiredFundsToFundsOut(Dictionary<CalendarPeriod, decimal> fundsOut, Dictionary<CalendarPeriod, decimal> expired, int expiryPeriod)
        {
            if (expired!=null && expired.Any(c=>c.Value> 0))
            {

                foreach (var expiredAmount in expired)
                {
                    var amount = expiredAmount.Value;

                    var fundsOutAvailable = fundsOut
                        .Where(c => new DateTime(c.Key.Year, c.Key.Month, 1) < new DateTime(expiredAmount.Key.Year, expiredAmount.Key.Month, 1)  && c.Value > 0)
                        .ToList();

                    foreach (var payment in fundsOutAvailable)
                    {
                        if (payment.Value >= amount)
                        {
                            fundsOut[payment.Key] = payment.Value - amount;
                            break;
                        }
                        amount = amount - payment.Value;
                        fundsOut[payment.Key] = 0;
                    }

                }
                
            }
        }

        private void CalculateAndApplyAdjustments(Dictionary<CalendarPeriod, decimal> fundsIn, int expiryPeriod)
        {
            if (fundsIn.Any(c => c.Value < 0))
            {
                var adjustmentsIn = fundsIn.Where(c => c.Value < 0).ToDictionary(key => key.Key, value => value.Value);

                foreach (var adjustment in adjustmentsIn.OrderBy(c => c.Key))
                {
                    var adjustmentStartPeriod = new DateTime(adjustment.Key.Year, adjustment.Key.Month, 1).AddMonths(expiryPeriod * -1);
                    var adjustmentEndPeriod = new DateTime(adjustment.Key.Year, adjustment.Key.Month, 1);
                    var adjustmentAmount = adjustment.Value * -1;

                    foreach (var fundsInValue in fundsIn.Where(c => c.Value > 0)
                                                        .ToDictionary(c => c.Key, c => c.Value)
                                                        .OrderBy(c => c.Key))
                    {
                        if (FundsAreInAdjustmentPeriod(fundsInValue, adjustmentStartPeriod, adjustmentEndPeriod))
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

        private static bool FundsAreInAdjustmentPeriod(KeyValuePair<CalendarPeriod, decimal> fundsInValue, DateTime adjustmentStartPeriod, DateTime adjustmentEndPeriod)
        {
            return new DateTime(fundsInValue.Key.Year, fundsInValue.Key.Month, 1) >= adjustmentStartPeriod &&
                   new DateTime(fundsInValue.Key.Year, fundsInValue.Key.Month, 1) <= adjustmentEndPeriod;
        }

        private static decimal CalculateExpiryAmount(IDictionary<CalendarPeriod, decimal> fundsOut, DateTime expiryDate, decimal expiryAmount)
        {
            var fundsOutAvailable = fundsOut
                .Where(c => new DateTime(c.Key.Year, c.Key.Month, 1) < expiryDate && c.Value > 0)
                .ToList();

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