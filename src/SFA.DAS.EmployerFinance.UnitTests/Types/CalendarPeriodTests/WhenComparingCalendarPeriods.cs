using System;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Types.Models;

namespace SFA.DAS.EmployerFinance.UnitTests.Types.CalendarPeriodTests
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

        [TestCase("2018-1", "2017-12", true)]
        [TestCase("2018-1", "2018-2", true)]
        [TestCase("2018-8", "2018-9", true)]
        [TestCase("2018-3", "2018-4", false)]
        [TestCase("2017-3", "2018-5", false)]
        public void Then_The_Tax_Years_Are_Compared_Correctly(string start, string end, bool expected)
        {
            //Arrange
            var startDate = new CalendarPeriod(Convert.ToInt32(start.Split('-')[0]), Convert.ToInt32(start.Split('-')[1]));
            var endDate = new CalendarPeriod(Convert.ToInt32(end.Split('-')[0]), Convert.ToInt32(end.Split('-')[1]));


            //Act
            var actual = startDate.AreSameTaxYear(endDate);

            Assert.AreEqual(expected, actual);
        }
    }
}
