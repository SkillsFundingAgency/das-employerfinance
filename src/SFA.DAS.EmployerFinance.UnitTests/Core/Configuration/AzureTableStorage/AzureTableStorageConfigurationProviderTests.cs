using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Core.Configuration.AzureTableStorage
{
    [TestFixture]
    [Parallelizable]
    public class AzureTableStorageConfigurationProviderTests : FluentTest<AzureTableStorageConfigurationProviderTestsFixture>
    {
        [Test]
        public void WhenReadingFlatJsonFromSingleTable_ThenConfigDataShouldBeCorrect()
        {
            Test(f => f.SetConfigs(new[] {("SFA.DAS.EmployerFinanceV2", "{\"key\": \"value\"}")}), f => f.Load(), f => f.AssertData(new[] {("SFA.DAS.EmployerFinanceV2:key", "value")} ));
        }
    }

    public class TestableAzureTableStorageConfigurationProvider : AzureTableStorageConfigurationProvider
    {
        public TestableAzureTableStorageConfigurationProvider(CloudStorageAccount cloudStorageAccount, string environment, IEnumerable<string> configNames)
            : base(cloudStorageAccount, environment, configNames)
        {
        }
        
        protected override TableOperation GetOperation(string serviceName)
        {
            var tableEntity = new Mock<IConfigurationRow>();
            tableEntity.SetupGet(te => te.RowKey).Returns(serviceName);

            var tableOperation = TableOperation.Retrieve<ConfigurationRow>("", "");
            typeof(TableOperation).GetProperty("Entity").SetValue(tableOperation, tableEntity.Object);
            return tableOperation;
        }
        
        public IDictionary<string, string> PublicData => Data;
    }
    
    public class AzureTableStorageConfigurationProviderTestsFixture
    {
        public const string EnvironmentName = "PROD";
        public const string ConfigurationTableName = "Configuration";
        public TestableAzureTableStorageConfigurationProvider ConfigProvider { get; set; }
        public Mock<CloudStorageAccount> CloudStorageAccount { get; set; }
        public Mock<CloudTableClient> CloudTableClient { get; set; }
        public Mock<CloudTable> CloudTable { get; set; }

        public AzureTableStorageConfigurationProviderTestsFixture()
        {
            var dummyUri = new Uri("http://example.com/");
            var dummyStorageCredentials = new StorageCredentials();
            CloudTable = new Mock<CloudTable>(dummyUri);
            CloudTableClient = new Mock<CloudTableClient>(dummyUri, dummyStorageCredentials);
            CloudTableClient.Setup(ctc => ctc.GetTableReference(ConfigurationTableName)).Returns(CloudTable.Object);
            CloudStorageAccount = new Mock<CloudStorageAccount>(dummyStorageCredentials, dummyUri, dummyUri, dummyUri, dummyUri);
            CloudStorageAccount.Setup(csa => csa.CreateCloudTableClient()).Returns(CloudTableClient.Object);
        }

        public void SetConfigs(IEnumerable<(string configKey, string json)> configs)
        {
            ConfigProvider = new TestableAzureTableStorageConfigurationProvider(CloudStorageAccount.Object, EnvironmentName, configs.Select(c => c.configKey));

            foreach (var config in configs)
            {
                var configurationRow = new AzureTableStorageConfigurationProvider.ConfigurationRow {Data = config.json};

                CloudTable.Setup(ct => ct.ExecuteAsync(It.Is<TableOperation>(to => to.Entity.RowKey == config.configKey)))
                    .ReturnsAsync(new TableResult { Result = configurationRow });
            }
        }

        public void Load()
        {
            ConfigProvider.Load();
        }

        public void AssertData(IEnumerable<(string key, string value)> expectedData)
        {
            var expectedDictionary = expectedData.ToDictionary(ed => ed.key, ed => ed.value);
            ConfigProvider.PublicData.Should().Equal(expectedDictionary);
        }
    }
}