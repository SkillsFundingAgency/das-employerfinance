using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerFinance.Web.Authorization;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class LocalAuthorizationHandlerTests : FluentTest<LocalAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsLocal_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetEnvironment(EnvironmentName.Development), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeOfType<AuthorizationResult>().Which.IsAuthorized.Should().BeTrue());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsNotLocal_ThenShouldReturnAuthorizationResult()
        {
            return TestAsync(f => f.SetEnvironment(EnvironmentName.Production).SetAuthorizationResult(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.AuthorizationResult));
        }
    }

    public class LocalAuthorizationHandlerTestsFixture
    {
        public IReadOnlyCollection<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler AuthorizationHandlerDecorator { get; set; }
        public Mock<IAuthorizationHandler> AuthorizationHandler { get; set; }
        public Mock<IHostingEnvironment> HostingEnvironment { get; set; }
        public AuthorizationResult AuthorizationResult { get; set; }

        public LocalAuthorizationHandlerTestsFixture()
        {
            Options = new [] { "" };
            AuthorizationContext = new AuthorizationContext();
            AuthorizationHandler = new Mock<IAuthorizationHandler>();
            HostingEnvironment = new Mock<IHostingEnvironment>();
            AuthorizationHandlerDecorator = new LocalAuthorizationHandler(HostingEnvironment.Object, AuthorizationHandler.Object);
            AuthorizationResult = new AuthorizationResult();
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return AuthorizationHandlerDecorator.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public LocalAuthorizationHandlerTestsFixture SetEnvironment(string environmentName)
        {
            HostingEnvironment.Setup(e => e.EnvironmentName).Returns(environmentName);
            
            return this;
        }

        public LocalAuthorizationHandlerTestsFixture SetAuthorizationResult()
        {
            AuthorizationHandler.Setup(h => h.GetAuthorizationResult(Options, AuthorizationContext)).ReturnsAsync(AuthorizationResult);
            
            return this;
        }
    }
}