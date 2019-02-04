using AutoMapper;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Mappings;

namespace SFA.DAS.EmployerFinance.UnitTests.Mappings
{
    [TestFixture]
    [Parallelizable]
    public class MappingsTests
    {
        [Test]
        public void AssertConfigurationIsValid_WhenAssertingConfigurationIsValid_ThenShouldNotThrowException()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(HealthCheckMappings)));

            config.AssertConfigurationIsValid();
        }
    }
}