using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
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
        public Mock<IMessageSession> MessageSession { get; }
        public Mock<INServiceBusHealthCheckResponseHandler> ResponseHandler { get; }
        public Mock<ILogger<NServiceBusHealthCheck>> Logger { get; }
        public NServiceBusHealthCheck NServiceBusHealthCheck { get; }
        public HealthCheckResult HealthCheckResult { get; private set; }

        public NServiceBusHealthCheckTestsFixture()
        {
            MessageSession = new Mock<IMessageSession>();
            Logger = new Mock<ILogger<NServiceBusHealthCheck>>();
            ResponseHandler = new Mock<INServiceBusHealthCheckResponseHandler>();
            NServiceBusHealthCheck = new NServiceBusHealthCheck(MessageSession.Object, ResponseHandler.Object, Logger.Object)
            {
                MessageResponseTimeoutMilliseconds = 10
            };
        }

        public async Task ReceiveResponseMessage()
        {
            MessageSession.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>()))
                          .Callback<object, SendOptions>((obj, opt) =>
            {
                if (obj is HealthCheckRequestMessage message)
                {
                    ResponseHandler.Raise(handler =>
                        handler.ReceivedResponse += null, ResponseHandler.Object, message.Id);
                }
            }).Returns(Task.CompletedTask);
            
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
        
        public async Task DoNotReceiveResponseMessage()
        {
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
        
        public async Task ExceptionThrownOnSend()
        {
            MessageSession.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>()))
                          .Throws<Exception>();
            
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
        
        public async Task ReceiveOtherResponseMessage()
        {
            MessageSession.Setup(s => s.Send(It.IsAny<object>(), It.IsAny<SendOptions>()))
                .Callback<object, SendOptions>((obj, opt) =>
                {
                    if (obj is HealthCheckRequestMessage message)
                    {
                        ResponseHandler.Raise(handler =>
                            handler.ReceivedResponse += null, ResponseHandler.Object, Guid.NewGuid());
                    }
                }).Returns(Task.CompletedTask);
            
            HealthCheckResult = await NServiceBusHealthCheck.CheckHealthAsync(new HealthCheckContext());
        }
    }
}