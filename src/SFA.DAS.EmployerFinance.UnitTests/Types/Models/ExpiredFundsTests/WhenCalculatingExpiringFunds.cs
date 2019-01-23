using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Types.Models;

namespace SFA.DAS.EmployerFinance.UnitTests.Types.Models.ExpiredFundsTests
{
    [Parallelizable]
    public class WhenCalculatingExpiringFunds
    {
        private ExpiredFunds _expiredFunds;
        private Dictionary<CalendarPeriod, decimal> _fundsIn;

        [SetUp]
        public void Arrange()
        {
            _expiredFunds = new ExpiredFunds();

            _fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), 5}
            };
        }

        [Test]
        public void Then_An_Exception_Is_Returned_When_Params_Are_Not_Supplied()
        {
            var actualFundsInException = Assert.Throws<ArgumentNullException>(() =>
                _expiredFunds.GetExpiringFunds(null, new Dictionary<CalendarPeriod, decimal>(),
                    new Dictionary<CalendarPeriod, decimal>(),24));
            Assert.IsTrue(actualFundsInException.Message.Contains("fundsIn"));
            var actualFundsOutException = Assert.Throws<ArgumentNullException>(() =>
                _expiredFunds.GetExpiringFunds(new Dictionary<CalendarPeriod, decimal>(), null,
                    new Dictionary<CalendarPeriod, decimal>(),24));
            Assert.IsTrue(actualFundsOutException.Message.Contains("fundsOut"));
        }

        [Test]
        public void Then_If_There_Are_No_Funds_In_Then_There_Is_No_Expiry()
        {
            //Act
            var actual = _expiredFunds.GetExpiringFunds(new Dictionary<CalendarPeriod, decimal>(), new Dictionary<CalendarPeriod, decimal>(), null, 3);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void Then_Expired_Funds_Are_Returned_If_There_Are_Funds_In_Depending_On_The_Expiry_Period()
        {
            //Arrange
            var expiryPeriod = 1;

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, new Dictionary<CalendarPeriod, decimal>(), null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }

        [Test]
        public void Then_If_I_Have_Funds_In_They_Are_Taken_Off_My_Expiry_Amount()
        {
            //Arrange
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(0, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }


        [Test]
        public void Then_If_I_Have_Refunds_In_They_Are_Taken_Off_My_Expiry_Amount()
        {
            //Arrange
            var expiryPeriod = 3;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), -5}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
            Assert.AreEqual(0, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(5, actual.Skip(2).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }


        [Test]
        public void Then_If_I_Have_Multiple_Refunds_In_They_Are_Taken_Off_My_Expiry_Amount()
        {
            //Arrange
            var expiryPeriod = 3;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), -5},
                {new CalendarPeriod(2018, 12), -2}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(6, actual.Count);
            Assert.AreEqual(0, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(5, actual.Skip(2).First().Value);
            Assert.AreEqual(8, actual.Skip(3).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }

        [Test]
        public void Then_The_Expiry_Date_Returned_Is_Correct_Based_On_The_Expiry_Period()
        {
            //Arrange
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(2018, actual.First().Key.Year);
            Assert.AreEqual(12, actual.First().Key.Month);
            Assert.AreEqual(2019, actual.Skip(1).First().Key.Year);
            Assert.AreEqual(1, actual.Skip(1).First().Key.Month);
        }


        [Test]
        public void Then_If_I_Have_More_Funds_In_They_Are_Taken_Off_My_Expiry_Amounts_For_Multiple_Months()
        {
            //Arrange
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 12}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(0, actual.First().Value);
            Assert.AreEqual(7, actual.Skip(1).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }


        [Test]
        public void Then_If_I_Have_A_Large_Carry_Over_Funds_In_They_Are_Taken_Off_My_Expiry_Amounts_For_Multiple_Months_And_Expiry_Periods()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), 5},
                {new CalendarPeriod(2019, 2), 5},
                {new CalendarPeriod(2019, 3), 5},
                {new CalendarPeriod(2020, 3), 5}
            };
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 11), 120}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(7, actual.Count);
            Assert.IsTrue(actual.All(c=>c.Value.Equals(0)));
        }

        [Test]
        public void Then_My_Funds_Out_Are_Taken_Off_The_Correct_Funds_In_Period()
        {
            //Arrange
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 1), 12}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(1, actual.Last().Value);
        }


        [Test]
        public void Then_If_I_Have_Multiple_Funds_Out_Are_Taken_Off_The_Correct_Funds_In_Period()
        {
            //Arrange
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 1), 12},
                {new CalendarPeriod(2019, 2), 3}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_If_I_Have_Multiple_Funds_Out_Not_For_The_Valid_Period_Then_Funds_Expire()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 1), 5},
                { new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2018, 10), 10}
            };
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 4), 12},
                {new CalendarPeriod(2019, 5), 3}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null,
                expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(8, actual.Skip(2).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }

        [Test]
        public void Then_If_I_Have_Seasonal_Values_The_Funds_Expire_Correctly()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 9), 10},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), 5},
                {new CalendarPeriod(2019, 4), 2}
            };
            var expiryPeriod = 2;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 1), 12},
                {new CalendarPeriod(2019, 2), 3}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(0, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Skip(3).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_If_I_Have_Adjustments_On_My_Funds_In_It_Is_Applied_In_Descending_Order_To_The_Funds_In()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), -10},
                {new CalendarPeriod(2019, 1), 5},
                {new CalendarPeriod(2019, 2), -5},
            };
            var expiryPeriod = 6;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {

            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
            Assert.AreEqual(9, actual.First().Value);
            Assert.AreEqual(0, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Skip(3).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }


        [Test]
        public void Then_If_I_Have_Large_Adjustments_On_My_Funds_In_It_Is_Applied_In_Descending_Order_For_That_Financial_Year()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 29},
                {new CalendarPeriod(2018, 12), -19},
                {new CalendarPeriod(2019, 1), 5},
                {new CalendarPeriod(2019, 6), -5},
            };
            var expiryPeriod = 24;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {

            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(10, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(5, actual.Skip(3).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_Only_Adjustments_In_The_Correct_Period_Are_Applied()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 12), 10},
                {new CalendarPeriod(2019, 12), -10}
            };
            var expiryPeriod = 12;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {

            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }


        [Test]
        public void Then_Adjustments_And_Funds_Out_Are_Applied_In_The_Correct_Periods()
        {
            //Arrange
            var fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 20},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), -5},
                {new CalendarPeriod(2019, 1), 5},
                {new CalendarPeriod(2019, 2), -5},
            };
            var expiryPeriod = 6;
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 1), 12},
                {new CalendarPeriod(2019, 2), 3}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(fundsIn, fundsOut, null, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
            Assert.AreEqual(5, actual.First().Value);
            Assert.AreEqual(4, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Skip(3).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_When_I_Already_Have_Expired_Funds_These_Are_Included_In_The_Calculation()
        {
            //Arrange
            var expiryPeriod = 2;
            _fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), 5}
            };
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 11), 12},
                {new CalendarPeriod(2019, 1), 3}
            };
            var expiredFunds = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 12), 9},
                {new CalendarPeriod(2019, 1), 4},
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, expiredFunds, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(9, actual.First().Value);
            Assert.AreEqual(4, actual.Skip(1).First().Value);
            Assert.AreEqual(5, actual.Skip(2).First().Value);
            Assert.AreEqual(5, actual.Last().Value);
        }

        [Test]
        public void Then_When_I_Already_Have_Expired_Funds_And_Adjustments_They_Are_Applied_To_The_None_Expired_FundsIn()
        {
            //Arrange
            var expiryPeriod = 4;
            _fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), -5},
                {new CalendarPeriod(2019, 2), 5},
                {new CalendarPeriod(2019, 3), -4}
            };
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 11), 12}, 
                {new CalendarPeriod(2019, 1), 2} 
            };
            var expiredFunds = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 2), 9},
                {new CalendarPeriod(2019, 3), 4},
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, expiredFunds, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(6, actual.Count);
            Assert.AreEqual(9, actual.First().Value);
            Assert.AreEqual(4, actual.Skip(1).First().Value);
            Assert.AreEqual(2, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Skip(3).First().Value);
            Assert.AreEqual(1, actual.Skip(4).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_When_I_Already_Have_Expired_Funds_And_Adjustments_And_Refunds_They_Are_Applied_To_The_None_Expired_FundsIn()
        {
            //Arrange
            var expiryPeriod = 4;
            _fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 10), 10},
                {new CalendarPeriod(2018, 11), 9},
                {new CalendarPeriod(2018, 12), 8},
                {new CalendarPeriod(2019, 1), -5},
                {new CalendarPeriod(2019, 2), 5},
                {new CalendarPeriod(2019, 3), -4}
            };
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2018, 11), 12}, 
                {new CalendarPeriod(2019, 1), 10}, 
                {new CalendarPeriod(2019, 2), -1} 
            };
            var expiredFunds = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 2), 10},
                {new CalendarPeriod(2019, 3), 9},
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, expiredFunds, expiryPeriod);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(7, actual.Count);
            Assert.AreEqual(10, actual.First().Value);
            Assert.AreEqual(9, actual.Skip(1).First().Value);
            Assert.AreEqual(0, actual.Skip(2).First().Value);
            Assert.AreEqual(0, actual.Skip(3).First().Value);
            Assert.AreEqual(1, actual.Skip(4).First().Value);
            Assert.AreEqual(0, actual.Last().Value);
        }

        [Test]
        public void Then_The_Balance_Is_Correctly_Calculated_Over_A_Large_Expiry_Period()
        {
            //Arrange
            var expiryPeriod = 24;
            _fundsIn = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2017, 5), 15000},
                {new CalendarPeriod(2017, 6), 14000},
                {new CalendarPeriod(2017, 7), 15000},
                {new CalendarPeriod(2017, 8), 10000},
                {new CalendarPeriod(2017, 9), 15000},
                {new CalendarPeriod(2017, 10), 12000},
                {new CalendarPeriod(2017, 11), 13000},
                {new CalendarPeriod(2017, 12), 12000},
                {new CalendarPeriod(2018, 1), 13000},
                {new CalendarPeriod(2018, 2), 15000},
                {new CalendarPeriod(2018, 3), 15000},
                {new CalendarPeriod(2018, 4), 13000},
                {new CalendarPeriod(2018, 5), 15000},
                {new CalendarPeriod(2018, 6), 10000},
                {new CalendarPeriod(2018, 7), 10000},
                {new CalendarPeriod(2018, 8), 15000},
                {new CalendarPeriod(2018, 9), 15000},
                {new CalendarPeriod(2018, 10), 15000},
                {new CalendarPeriod(2018, 11), 15000},
                {new CalendarPeriod(2018, 12), 15000},
                {new CalendarPeriod(2019, 1), 15000},
                {new CalendarPeriod(2019, 2), 15000},
                {new CalendarPeriod(2019, 3), 15000},
                {new CalendarPeriod(2019, 4), 15000},
                {new CalendarPeriod(2019, 5), 15000},
                {new CalendarPeriod(2019, 6), -2000},
                {new CalendarPeriod(2019, 7), -2000},
            };
            var fundsOut = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2017, 9), 300}, 
                {new CalendarPeriod(2017, 10), 300},
                {new CalendarPeriod(2017, 11), 300},
                {new CalendarPeriod(2017, 12), 300},
                {new CalendarPeriod(2018, 1), 300}, 
                {new CalendarPeriod(2018, 2), 300}, 
                {new CalendarPeriod(2018, 3), 300}, 
                {new CalendarPeriod(2018, 4), 300}, 
                {new CalendarPeriod(2018, 5), 300}, 
                {new CalendarPeriod(2018, 6), 300}, 
                {new CalendarPeriod(2018, 7), 300}, 
                {new CalendarPeriod(2018, 8), 300}, 
                {new CalendarPeriod(2018, 9), 300}, 
                {new CalendarPeriod(2018, 10), 300},
                {new CalendarPeriod(2018, 11), 300},
                {new CalendarPeriod(2018, 12), 300},
                {new CalendarPeriod(2019, 1), 300}, 
                {new CalendarPeriod(2019, 2), 300}, 
                {new CalendarPeriod(2019, 3), 300}, 
                {new CalendarPeriod(2019, 4), 300}, 
                {new CalendarPeriod(2019, 5), 300},
                {new CalendarPeriod(2019, 6), 300} ,
                {new CalendarPeriod(2019, 7), 500}, 
                {new CalendarPeriod(2019, 8), 500}
            };
            var expired = new Dictionary<CalendarPeriod, decimal>
            {
                {new CalendarPeriod(2019, 5), 8700} ,
                {new CalendarPeriod(2019, 6), 13700},
                {new CalendarPeriod(2019, 7), 12500}
            };

            //Act
            var actual = _expiredFunds.GetExpiringFunds(_fundsIn, fundsOut, expired, expiryPeriod);

            //Assert
            Assert.AreEqual(9500, actual.Skip(3).First().Value);
        }
    }
}