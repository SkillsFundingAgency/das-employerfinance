using AutoMapper;
using NUnit.Framework;

namespace SFA.DAS.EmployerFinance.UnitTests.Mappings
{
    [TestFixture]
    [Parallelizable]
    public class MappingsTests
    {
        [Test]
        [Ignore("Currently we have no mapping to test")]
        public void AssertConfigurationIsValid_WhenAssertingConfigurationIsValid_ThenShouldNotThrowException()
        {
            var config = new MapperConfiguration(c => {});
            
            config.AssertConfigurationIsValid();
        }
    }
}