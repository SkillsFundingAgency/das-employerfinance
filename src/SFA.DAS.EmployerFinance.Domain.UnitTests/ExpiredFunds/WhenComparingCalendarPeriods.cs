using NUnit.Framework;
using SFA.DAS.EmployerFinance.Domain.ExpiredFunds;

namespace SFA.DAS.EmployerFinance.Domain.UnitTests.ExpiredFunds
{
    public class WhenComparingCalendarPeriods
    {
        [TestCase(2018,01,true)]
        [TestCase(2018,02,true)]
        [TestCase(2017,12,false)]
        [TestCase(2019,12,true)]
        public void Then_The_Greater_Than_Or_Equals_Comparison_Is_Correct(int year, int month, bool expected)
        {
            //Arrange
            var calendarPeriod1 = new CalendarPeriod(year, month);
            var calendarPeriod2 = new CalendarPeriod(2018, 01);

            var actual = calendarPeriod1 >= calendarPeriod2;

            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestCase(2018, 01, false)]
        [TestCase(2018, 02, true)]
        [TestCase(2017, 12, false)]
        [TestCase(2019, 12, true)]
        public void Then_The_Greater_Than_Comparison_Is_Correct(int year, int month, bool expected)
        {
            //Arrange
            var calendarPeriod1 = new CalendarPeriod(year, month);
            var calendarPeriod2 = new CalendarPeriod(2018, 01);

            var actual = calendarPeriod1 > calendarPeriod2;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(2018, 01, true)]
        [TestCase(2018, 02, false)]
        [TestCase(2017, 12, true)]
        [TestCase(2016, 12, true)]
        public void Then_The_Less_Than_Or_Equals_Comparison_Is_Correct(int year, int month, bool expected)
        {
            //Arrange
            var calendarPeriod1 = new CalendarPeriod(year, month);
            var calendarPeriod2 = new CalendarPeriod(2018, 01);

            var actual = calendarPeriod1 <= calendarPeriod2;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestCase(2018, 01, false)]
        [TestCase(2018, 02, false)]
        [TestCase(2017, 12, true)]
        [TestCase(2016, 12, true)]
        public void Then_The_Less_Than_Comparison_Is_Correct(int year, int month, bool expected)
        {
            //Arrange
            var calendarPeriod1 = new CalendarPeriod(year, month);
            var calendarPeriod2 = new CalendarPeriod(2018, 01);

            var actual = calendarPeriod1 < calendarPeriod2;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
