using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.DependencyResolution
{
    [TestFixture]
    [Parallelizable]
    public class IocTests
    {
        [Test]
        public void WhenIocIsInitializationThenContainerShouldBeValid()
        {
            var registry = new Registry();
            IoC.Initialize(registry);
            var container = new Container(registry);
            container.AssertConfigurationIsValid();
        }
    }
}