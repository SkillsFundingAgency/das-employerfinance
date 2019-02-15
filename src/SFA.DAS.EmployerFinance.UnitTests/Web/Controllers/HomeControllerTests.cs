using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.Controllers;
using SFA.DAS.EmployerFinance.Web.Urls;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HomeControllerTests : FluentTest<HomeControllerTestsFixture>
    {
        [Test]
        public void Index_WhenGettingIndexAction_ThenShouldRedirectToEmployerPortal()
        {
            Test(f => f.SetCurrentEnvironmentIsLocal(false), f => f.Local(), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == HomeControllerTestsFixture.EmployerPortalUrl));
        }
    }

    public class HomeControllerTestsFixture
    {
        public HomeController HomeController { get; set; }
        public Mock<IHostingEnvironment> HostingEnvironment { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        
        public const string EmployerPortalUrl = "https://foo.bar";

        public HomeControllerTestsFixture()
        {
            HostingEnvironment = new Mock<IHostingEnvironment>();
            EmployerUrls = new Mock<IEmployerUrls>();

            EmployerUrls.Setup(au => au.Homepage()).Returns(EmployerPortalUrl);
            
            HomeController = new HomeController(HostingEnvironment.Object, EmployerUrls.Object);
        }

        public IActionResult Local()
        {
            return HomeController.Index();
        }

        public HomeControllerTestsFixture SetCurrentEnvironmentIsLocal(bool isLocal)
        {
            var environmentName = isLocal ? EnvironmentName.Development : EnvironmentName.Production;
            
            HostingEnvironment.Setup(e => e.EnvironmentName).Returns(environmentName);
            
            return this;
        }
    }
}