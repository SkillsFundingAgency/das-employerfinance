using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.DependencyResolution
{
    [TestFixture]
    [Parallelizable]
    public class IocTests
    {
        [Test, Ignore("Fails on build server as doesn't have access to config")]
        public void WhenIocIsInitializationThenContainerShouldBeValid()
        {
            var registry = new Registry();
            IoC.Initialize(registry, Mock.Of<IConfiguration>());
            var container = new Container(registry);
            container.AssertConfigurationIsValid();
        }
    }
}