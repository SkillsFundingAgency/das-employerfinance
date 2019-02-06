using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Moq;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.DependencyResolution
{
    [TestFixture]
    [Parallelizable]
    public class IocTests
    {
        //todo: fix test
        [Test, Ignore("Fails 1) everywhere as need to somehow mock IContext supplied by For() method 2) on build server as doesn't have access to config")]
        public void WhenIocIsInitializationThenContainerShouldBeValid()
        {
            using (var container = IoC.Initialize(new List<ServiceDescriptor> {ServiceDescriptor.Singleton(Mock.Of<IConfiguration>())}))
            {
                container.AssertConfigurationIsValid();
            }
        }
    }
}