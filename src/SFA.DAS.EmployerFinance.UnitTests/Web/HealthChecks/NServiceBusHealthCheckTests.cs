using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NServiceBus.Callbacks.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Messages.Messages;
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
        public Task WhenSendingHealthCheckRequest_ShouldGetHealthyStatusIfResponseReceived()
        {
            return TestAsync(f => f.ReceiveResponseMessage(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Healthy));
        }
        
        [Test]
        public Task WhenSendingHealthCheckRequest_ShouldGetUnhealthyStatusIfResponseReceivedIsOtherMessage()
        {
            return TestAsync(f => f.ReceiveOtherResponseMessage(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task WhenSendingHealthCheckRequest_ShouldGetUnHealthyStatusIfResponseNotReceived()
        {
            return TestAsync(f => f.DoNotReceiveResponseMessage(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task WhenSendingHealthCheckRequest_ShouldGetUnHealthyStatusIfExceptionThrown()
        {
            return TestAsync(f => f.ExceptionThrownOnSend(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
    }

    public class NServiceBusHealthCheckTestsFixture
    {
        public TestableCallbackAwareSession MessageSession { get; }
        public Mock<ILogger<NServiceBusHealthCheck>> Logger { get; }
        public NServiceBusHealthCheck NServiceBusHealthCheck { get; }
        public HealthCheckResult HealthCheckResult { get; private set; }

        public NServiceBusHealthCheckTestsFixture()
        {
            MessageSession = new TestableCallbackAwareSession();
            Logger = new Mock<ILogger<NServiceBusHealthCheck>>();

            NServiceBusHealthCheck = new NServiceBusHealthCheck(MessageSession, Logger.Object, 500);
        }

        public async Task ReceiveResponseMessage()
        {
            MessageSession.When(matcher: (HealthCheckRequestMessage message) => true, response: HealthStatus.Healthy);
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
        
        public async Task DoNotReceiveResponseMessage()
        {
            var tokenSource = new CancellationTokenSource();

            MessageSession.When(matcher: (HealthCheckRequestMessage message) =>
            {
                tokenSource.Cancel();
                return false;
            }, response: HealthStatus.Healthy);
            
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext(), tokenSource.Token);
        }
        
        public async Task ExceptionThrownOnSend()
        {
            var messageSessionMock = new Mock<IMessageSession>();
            messageSessionMock.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>()))
                              .Throws<Exception>();

            var exceptionHealthChecker = new NServiceBusHealthCheck(messageSessionMock.Object, Logger.Object);
            HealthCheckResult = await exceptionHealthChecker.CheckHealthAsync(new HealthCheckContext());
        }
        
        public async Task ReceiveOtherResponseMessage()
        {           
            MessageSession.When(matcher: (HealthCheckRequestMessage message) => true, response: HealthStatus.Unhealthy);
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
    }
}