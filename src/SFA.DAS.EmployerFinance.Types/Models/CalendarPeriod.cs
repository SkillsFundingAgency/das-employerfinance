using System;

namespace SFA.DAS.EmployerFinance.Types.Models
{
    public class CalendarPeriod : IComparable<CalendarPeriod>
    {
        public CalendarPeriod(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public int Year { get; }
        public int Month { get; }

        public int CompareTo(CalendarPeriod compareTo)
        {
            return Compare(this, compareTo);
        }

        public static bool operator >(CalendarPeriod period1, CalendarPeriod period2)
        {
            return Compare(period1, period2) > 0;
        }

        public static bool operator <(CalendarPeriod period1, CalendarPeriod period2)
        {
            return Compare(period1, period2) < 0;
        }

        public static bool operator <=(CalendarPeriod period1, CalendarPeriod period2)
        {
            return Compare(period1, period2) <= 0;
        }

        public static bool operator >=(CalendarPeriod period1, CalendarPeriod period2)
        {
            return Compare(period1, period2) >= 0;
        }

        public bool AreSameTaxYear(CalendarPeriod compareTo)
        {
            return CheckPeriodsAreInSameTaxYear(new DateTime(Year, Month, 1), new DateTime(compareTo.Year, compareTo.Month, 1));
        }

        private static bool CheckPeriodsAreInSameTaxYear(DateTime firstPeriod, DateTime secondPeriod)
        {
            var startPeriodTaxYear = GetTaxYearFromDate(firstPeriod);

            var endPeriodTaxYear = GetTaxYearFromDate(secondPeriod);

            return startPeriodTaxYear == endPeriodTaxYear;
        }

        private static int GetTaxYearFromDate(DateTime firstPeriod)
        {
            return firstPeriod.Month >= 1 && firstPeriod.Month < 4
                ? firstPeriod.Year -1
                : firstPeriod.Year;
        }

        private static int Compare(CalendarPeriod calendarPeriod1, CalendarPeriod calendarPeriod2)
        {
            if (calendarPeriod1 == null || calendarPeriod2 == null)
            {
                return 0;
            }

            if (calendarPeriod1.Year > calendarPeriod2.Year)
            {
                return 1;
            }

            if (calendarPeriod1.Year < calendarPeriod2.Year)
            {
                return -1;
            }

            if (calendarPeriod1.Year == calendarPeriod2.Year)
            {
                if (calendarPeriod1.Month > calendarPeriod2.Month)
                {
                    return 1;
                }

                if (calendarPeriod1.Month < calendarPeriod2.Month)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}