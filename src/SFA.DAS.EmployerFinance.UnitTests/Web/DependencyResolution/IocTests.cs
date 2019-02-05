using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.DependencyResolution
{
    [TestFixture]
    [Parallelizable]
    public class IocTests
    {
        [Test, Ignore("Fails on build server as doesn't have access to config")]
        public void WhenIocIsInitializationThenContainerShouldBeValid()
        {
            //todo: once DI has been fixed
//            using (var container = IoC.Initialize(new List<ServiceDescriptor>(), Mock.Of<IConfiguration>()))
//            {
//                container.AssertConfigurationIsValid();
//            }
        }
    }
}