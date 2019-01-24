using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.UnitTests.Configuration
{
    public class HostEnvironmentSettingsFactoryUnitTests
    {
        private Mock<IEnvironmentService> _environmentService;
        
        [SetUp]
        public void Setup()
        {
            _environmentService = new Mock<IEnvironmentService>();
        }
        
        [Test]
        public void ReturnsProductionEnvironmentNameInNonLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(false);

            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual("Production", settings.EnvironmentName);
        }
        
        [Test]
        public void ReturnsDevelopmentEnvironmentNameInLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(true);

            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual("Development", settings.EnvironmentName);
        }

        [Test]
        public void ReturnsCorrectAppSettingFilePathInNonLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(false);

            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual("appsettings.json", settings.AppSettingsFilePath);
        }
        
        [Test]
        public void ReturnsCorrectAppSettingFilePathInLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(true);

            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual("appsettings.Development.json", settings.AppSettingsFilePath);
        }
        
        [Test]
        public void ReturnsLogLevelSetInAppSettingsInNonLocal()
        {
            const LogLevel expectedLogLevel = LogLevel.Trace;
            var expectedLogLevelName = Enum.GetName(typeof(LogLevel), expectedLogLevel);
            
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(false);
            _environmentService.Setup(x => x.GetVariable(AppSettingsKeys.LogLevel)).Returns(expectedLogLevelName);
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(expectedLogLevel, settings.MinLogLevel);
        }
        
        [Test]
        public void ReturnsLogLevelSetInAppSettingsInLocal()
        {
            const LogLevel expectedLogLevel = LogLevel.Trace;
            var expectedLogLevelName = Enum.GetName(typeof(LogLevel), expectedLogLevel);
            
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(true);
            _environmentService.Setup(x => x.GetVariable(AppSettingsKeys.LogLevel)).Returns(expectedLogLevelName);
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(expectedLogLevel, settings.MinLogLevel);
        }
        
        [Test]
        public void ReturnsDebugLogLevelIfNoAppSettingsInNonLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(false);
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(LogLevel.Debug, settings.MinLogLevel);
        }
        
        [Test]
        public void ReturnsDebugLogLevelIfNoAppSettingsInLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(true);
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(LogLevel.Debug, settings.MinLogLevel);
        }
        
        [Test]
        public void ReturnsDebugLogLevelIfInvalidAppSettingsInNonLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(false);
            _environmentService.Setup(x => x.GetVariable(AppSettingsKeys.LogLevel)).Returns("invalid");
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(LogLevel.Debug, settings.MinLogLevel);
        }
        
        [Test]
        public void ReturnsDebugLogLevelIfInvalidAppSettingsInLocal()
        {
            _environmentService.Setup(x => x.IsCurrent(DasEnv.LOCAL)).Returns(true);
            _environmentService.Setup(x => x.GetVariable(AppSettingsKeys.LogLevel)).Returns("invalid");
            
            var settings = HostEnvironmentSettingsFactory.Create(_environmentService.Object);
            
            Assert.AreEqual(LogLevel.Debug, settings.MinLogLevel);
        }
    }
}