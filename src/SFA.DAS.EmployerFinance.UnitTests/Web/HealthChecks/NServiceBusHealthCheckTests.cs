using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.HealthChecks;
using SFA.DAS.Testing;
using Xunit.Extensions.AssertExtensions;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.HealthChecks
{
    [TestFixture]
    [Parallelizable]
    public class NServiceBusHealthCheckTests : FluentTest<NServiceBusHealthCheckTestsFixture>
    {
        [Test]
        public Task CheckHealthAsync_WhenSendSucceeds_ThenShouldShouldReturnHealthyStatus()
        {
            return TestAsync(
                f => f.SetSendSuccess(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.ShouldEqual(HealthStatus.Healthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenSendFails_ThenShouldReturnUnhealthyStatus()
        {
            return TestAsync(
                f => f.SetSendFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenSendFails_ThenShouldReturnException()
        {
            return TestAsync(
                f => f.SetSendFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
    }

    public class NServiceBusHealthCheckTestsFixture
    {
        public HealthCheckContext HealthCheckContext { get; set; }
        public Mock<IMessageSession> MessageSession { get; set; }
        public Mock<ILogger<NServiceBusHealthCheck>> Logger { get; set; }
        public NServiceBusHealthCheck NServiceBusHealthCheck { get; set; }
        public Exception Exception { get; set; }

        public NServiceBusHealthCheckTestsFixture()
        {
            HealthCheckContext = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("Foo", Mock.Of<IHealthCheck>(), null, null)
            };
            
            MessageSession = new Mock<IMessageSession>();
            Logger = new Mock<ILogger<NServiceBusHealthCheck>>();
            NServiceBusHealthCheck = new NServiceBusHealthCheck(MessageSession.Object, Logger.Object);
        }

        public Task<HealthCheckResult> CheckHealthAsync()
        {
            return NServiceBusHealthCheck.CheckHealthAsync(HealthCheckContext);
        }

        public NServiceBusHealthCheckTestsFixture SetSendSuccess()
        {
            MessageSession.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>())).Returns(Task.CompletedTask);
            
            return this;
        }

        public NServiceBusHealthCheckTestsFixture SetSendFailure()
        {
            MessageSession.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>())).ThrowsAsync(Exception);
            
            return this;
        }
    }
}