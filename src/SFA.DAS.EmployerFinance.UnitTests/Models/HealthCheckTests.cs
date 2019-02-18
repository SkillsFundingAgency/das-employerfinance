using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class HealthCheckTests : FluentTest<HealthCheckTestsFixture>
    {
        [Test]
        public void New_WhenCreatingAHealthCheck_ThenShouldCreateAHealthCheck()
        {
            Test(f => f.New(), (f, r) =>
            {
                r.Should().NotBeNull();
            });
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetPublishedEmployerFinanceEventProperty()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.PublishedEmployerFinanceEvent >= f.PreRun && h.PublishedEmployerFinanceEvent <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldPublishAHealthCheckEvent()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.UnitOfWorkContext.GetEvents().Should().HaveCount(1)
                .And.AllBeOfType<HealthCheckEvent>()
                .And.AllBeEquivalentTo(new HealthCheckEvent(f.HealthCheck.Id, f.HealthCheck.PublishedEmployerFinanceEvent)));
        }

        [Test]
        public void ReceiveEvent_WhenReceivingAHealthCheckEvent_ThenShouldSetReceivedEmployerFinanceEventProperty()
        {
            Test(f => f.SetHealthCheck(), f => f.ReceiveEvent(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedEmployerFinanceEvent >= f.PreRun && h.ReceivedEmployerFinanceEvent <= f.PostRun));
        }
    }

    public class HealthCheckTestsFixture
    {
        public HealthCheck HealthCheck { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public Func<Task> ApprenticeshipInfoServiceApiRequest { get; set; }
        public Func<Task> EmployerFinanceApiRequest { get; set; }
        public DateTime? PreRun { get; set; }
        public DateTime? PostRun { get; set; }

        public HealthCheckTestsFixture()
        {
            UnitOfWorkContext = new UnitOfWorkContext();
            ApprenticeshipInfoServiceApiRequest = () => Task.CompletedTask;
            EmployerFinanceApiRequest = () => Task.CompletedTask;
        }

        public HealthCheck New()
        {
            return new HealthCheck();
        }

        public async Task Run()
        {
            PreRun = DateTime.UtcNow;

            await HealthCheck.Run(EmployerFinanceApiRequest);

            PostRun = DateTime.UtcNow;
        }

        public void ReceiveEvent()
        {
            PreRun = DateTime.UtcNow;

            HealthCheck.ReceiveEmployerFinanceEvent();

            PostRun = DateTime.UtcNow;
        }

        public HealthCheckTestsFixture SetHealthCheck()
        {
            HealthCheck = ObjectActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 1);

            return this;
        }

        public HealthCheckTestsFixture SetApprenticeshipInfoServiceApiRequestException()
        {
            ApprenticeshipInfoServiceApiRequest = () => throw new Exception();

            return this;
        }
    }
}