using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.EmployerFinance.Jobs.ScheduledJobs;
using SFA.DAS.EmployerFinance.Services;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Jobs.ScheduledJobs
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsJobTests : FluentTest<ProcessLevyDeclarationsJobTestsFixture>
    {
        [Test]
        public Task Run_WhenJobIsRun_ThenShouldSendProcessLevyDeclarationsCommandWithCurrentPayrollPeriodValue()
        {
            return TestAsync(f => f.Run(), f => f.MessageSession
                .SentMessages
                .Select(m => m.Message)
                .Should().HaveCount(1)
                .And.Subject.Cast<ProcessLevyDeclarationsCommand>()
                .Should().Contain(c => c.PayrollPeriod == f.PayrollPeriod));
        }
    }

    public class ProcessLevyDeclarationsJobTestsFixture
    {
        public DateTime Now { get; set; }
        public DateTime Today { get; set; }
        public DateTime Month { get; set; }
        public DateTime PayrollPeriod { get; set; }
        public TestableMessageSession MessageSession { get; set; }
        public Mock<IDateTimeService> DateTimeService { get; set; }
        public Mock<ILogger> Logger { get; set; }
        public ProcessLevyDeclarationsJob Job { get; set; }

        public ProcessLevyDeclarationsJobTestsFixture()
        {
            Now = DateTime.UtcNow;
            Today = Now.Date;
            Month = new DateTime(Today.Year, Today.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            PayrollPeriod = Month.AddMonths(-1);
            MessageSession = new TestableMessageSession();
            DateTimeService = new Mock<IDateTimeService>();
            Logger = new Mock<ILogger>();

            DateTimeService.Setup(s => s.UtcNow).Returns(Now);
            
            Job = new ProcessLevyDeclarationsJob(MessageSession, DateTimeService.Object);
        }

        public Task Run()
        {
            return Job.Run(null, Logger.Object);
        }
    }
}