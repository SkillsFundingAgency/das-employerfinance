using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.Extensions;
using SFA.DAS.EmployerFinance.Web.Urls;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.Extensions
{
    [TestFixture]
    [Parallelizable]
    public class HtmlHelperExtensionsTests: FluentTest<HtmlHelperExtensionsTestsFixture>
    {
        [Test]
        public void WhenGettingEmployerUrls_ThenShouldGetEmployerUrls()
        {
            Test(f => f.GetEmployerUrls(), (f, r) => r.Should().Be(f.EmployerUrls.Object));
        }
    }

    public class HtmlHelperExtensionsTestsFixture
    {
        public readonly ViewDataDictionary ViewDataDictionary;
        public readonly Mock<IEmployerUrls> EmployerUrls;
        public readonly Mock<IHtmlHelper> HtmlHelper;
            
        public HtmlHelperExtensionsTestsFixture()
        {
            ViewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            EmployerUrls = new Mock<IEmployerUrls>();
            ViewDataDictionary["EmployerUrls"] = EmployerUrls.Object;
            HtmlHelper = new Mock<IHtmlHelper>();
            HtmlHelper.SetupGet(hh => hh.ViewData).Returns(ViewDataDictionary);
        }

        public IEmployerUrls GetEmployerUrls()
        {
            return HtmlHelper.Object.EmployerUrls();
        }
    }
}