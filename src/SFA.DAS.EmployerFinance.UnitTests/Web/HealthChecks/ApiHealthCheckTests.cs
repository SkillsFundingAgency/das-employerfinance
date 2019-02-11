using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Api.Client.Http;
using SFA.DAS.EmployerFinance.Web.HealthChecks;
using SFA.DAS.Testing;
using Xunit.Extensions.AssertExtensions;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.HealthChecks
{
    [TestFixture]
    [Parallelizable]
    public class ApiHealthCheckTests : FluentTest<ApiHealthCheckTestsFixture>
    {
        [Test]
        public Task WhenPingingApi_ShouldGetHealthyStatusIfNoIssuesOccur()
        {
            return TestAsync(f => f.PingApiSuccessfully(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Healthy));
        }
        
        [Test]
        public Task WhenPingingApi_ShouldGetFailureStatusIfIssuesOccur()
        {
            return TestAsync(f => f.PingApiFailure(), 
                f => f.HealthCheckResult.Status.ShouldEqual(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task WhenPingingApi_ShouldGetExceptionDetailsIfIssuesOccur()
        {
            return TestAsync(f => f.PingApiFailure(), 
                f => f.HealthCheckResult.Exception.ShouldEqual(f.RestException));
        }
    }

    public class ApiHealthCheckTestsFixture
    {
        public Mock<IEmployerFinanceApiClient> Client { get; }
        public Mock<ILogger> Logger { get; }
        public ApiHealthCheck ApiHealthCheck { get; }

        public HealthCheckResult HealthCheckResult { get; private set; }

        public RestHttpClientException RestException { get; }

        public ApiHealthCheckTestsFixture()
        {
            Client = new Mock<IEmployerFinanceApiClient>();
            Logger = new Mock<ILogger>();
            
            ApiHealthCheck = new ApiHealthCheck(Client.Object, Logger.Object);

            var httpException = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = new HttpRequestMessage(),
                ReasonPhrase = "Url not found"
            };
            
            RestException = new RestHttpClientException(httpException, "Url not found");
        }

        public async Task PingApiSuccessfully()
        {
            Client.Setup(c => c.Ping()).ReturnsAsync("Healthy");
            HealthCheckResult = await ApiHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());
        }
        
        public async Task PingApiFailure()
        {
            Client.Setup(c => c.Ping()).ThrowsAsync(RestException);
            HealthCheckResult = await ApiHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());
        }
    }
}