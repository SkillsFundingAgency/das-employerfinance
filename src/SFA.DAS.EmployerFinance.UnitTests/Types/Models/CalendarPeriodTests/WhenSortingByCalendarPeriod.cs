using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Types.Models;

namespace SFA.DAS.EmployerFinance.UnitTests.Types.Models.CalendarPeriodTests
{
    public class WhenSortingByCalendarPeriod
    {
        [Test]
        public void Then_Calendar_Periods_Are_Sorted_Ascending_For_Months()
        {
            //Arrange
            var calendarList = new List<CalendarPeriod>
            {
                new CalendarPeriod(2018,05),
                new CalendarPeriod(2018,01),
                new CalendarPeriod(2018,03)
            };

            //Act
            var actual = calendarList.OrderBy(c => c).ToList();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(3,actual.Count);
            Assert.AreEqual(1,actual[0].Month);
            Assert.AreEqual(3, actual[1].Month);
            Assert.AreEqual(5, actual[2].Month);
        }

        [Test]
        public void Then_Calendar_Periods_Are_Sorted_Ascending_For_Years()
        {
            //Arrange
            var calendarList = new List<CalendarPeriod>
            {
                new CalendarPeriod(2018,01),
                new CalendarPeriod(2019,01),
                new CalendarPeriod(2020,01)
            };

            //Act
            var actual = calendarList.OrderBy(c => c).ToList();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(2018, actual[0].Year);
            Assert.AreEqual(2019, actual[1].Year);
            Assert.AreEqual(2020, actual[2].Year);
        }


        [Test]
        public void Then_Calendar_Periods_Are_Sorted_Ascending_For_Years_And_Months()
        {
            //Arrange
            var calendarList = new List<CalendarPeriod>
            {
                new CalendarPeriod(2020,03),
                new CalendarPeriod(2018,11),
                new CalendarPeriod(2018,9),
                new CalendarPeriod(2020,01)
            };

            //Act
            var actual = calendarList.OrderBy(c => c).ToList();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(9, actual[0].Month);
            Assert.AreEqual(2018, actual[0].Year);
            Assert.AreEqual(11, actual[1].Month);
            Assert.AreEqual(2018, actual[1].Year);
            Assert.AreEqual(1, actual[2].Month);
            Assert.AreEqual(2020, actual[2].Year);
            Assert.AreEqual(3, actual[3].Month);
            Assert.AreEqual(2020, actual[3].Year);
        }
    }
}
