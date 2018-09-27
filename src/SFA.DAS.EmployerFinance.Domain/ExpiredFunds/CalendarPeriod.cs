using System;

namespace SFA.DAS.EmployerFinance.Domain.ExpiredFunds
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