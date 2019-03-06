using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Web.HealthChecks;
using SFA.DAS.Http;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.HealthChecks
{
    [TestFixture]
    [Parallelizable]
    public class ApiHealthCheckTests : FluentTest<ApiHealthCheckTestsFixture>
    {
        [Test]
        public Task CheckHealthAsync_WhenPingSucceeds_ThenShouldReturnHealthyStatus()
        {
            return TestAsync(
                f => f.SetPingSuccess(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.Should().Be(HealthStatus.Healthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenPingFails_ThenShouldReturnUnhealthyStatus()
        {
            return TestAsync(
                f => f.SetPingFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.Should().Be(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenPingFails_ThenShouldReturnException()
        {
            return TestAsync(
                f => f.SetPingFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Exception.Should().Be(f.Exception));
        }
    }

    public class ApiHealthCheckTestsFixture
    {
        public Mock<IEmployerFinanceApiClient> ApiClient { get; set; }
        public Mock<ILogger<ApiHealthCheck>> Logger { get; set; }
        public ApiHealthCheck ApiHealthCheck { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public RestHttpClientException Exception { get; set; }

        public ApiHealthCheckTestsFixture()
        {
            ApiClient = new Mock<IEmployerFinanceApiClient>();
            Logger = new Mock<ILogger<ApiHealthCheck>>();
            ApiHealthCheck = new ApiHealthCheck(ApiClient.Object, Logger.Object);

            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = new HttpRequestMessage(),
                ReasonPhrase = "Url not found"
            };
            
            Exception = new RestHttpClientException(HttpResponseMessage, "Url not found");
        }

        public Task<HealthCheckResult> CheckHealthAsync()
        {
            return ApiHealthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);
        }

        public ApiHealthCheckTestsFixture SetPingSuccess()
        {
            ApiClient.Setup(c => c.Ping()).Returns(Task.CompletedTask);
            
            return this;
        }

        public ApiHealthCheckTestsFixture SetPingFailure()
        {
            ApiClient.Setup(c => c.Ping()).ThrowsAsync(Exception);
            
            return this;
        }
    }
}