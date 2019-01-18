using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.Extensions;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.Extensions
{
    [TestFixture]
    public class HtmlHelperExtensionsTests
    {
        [Test]
        public void WhenGettingEmployerUrls_ThenShouldGetEmployerUrls()
        {
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            var mockEmployerUrls = new Mock<IEmployerUrls>();
            viewData["EmployerUrls"] = mockEmployerUrls.Object;
            var mockHtmlHelper = new Mock<IHtmlHelper>();
            mockHtmlHelper.SetupGet(hh => hh.ViewData).Returns(viewData);
            
            var employerUrlsResult = mockHtmlHelper.Object.EmployerUrls();

            employerUrlsResult.Should().Be(mockEmployerUrls.Object);
        }
    }
}