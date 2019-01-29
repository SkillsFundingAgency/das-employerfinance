using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        public void x()
        {
            Test(f => f.SetConfigs(new[] {("SFA.DAS.EmployerFinanceV2", "{\"key\": \"value\"}")}), f => f.Load());
        }
    }

    public class TestableAzureTableStorageConfigurationProvider : AzureTableStorageConfigurationProvider
    {
        public TestableAzureTableStorageConfigurationProvider(CloudStorageAccount cloudStorageAccount, string environment, IEnumerable<string> configNames)
            : base(cloudStorageAccount, environment, configNames)
        {
        }

        public ImmutableDictionary<string, string> Configs;
        
        public void Test_SetConfigs(IEnumerable<(string configKey, string json)> configs)
        {
            Configs = configs.ToImmutableDictionary(config => config.configKey, config => config.json);
        }
        
        protected override TableOperation GetOperation(string serviceName)
        {
            //todo: don't need this first stuff (unless remove executeasync setup
            var tableEntity = new Mock<IConfigurationRow>();
            //tableEntity.SetupGet(te => te.RowKey).Returns(Configs[serviceName]);
            tableEntity.SetupGet(te => te.RowKey).Returns(serviceName);
            //todo: why do we set json below?
            tableEntity.SetupGet(te => te.Data).Returns("{\"\",\"\")");

//            var tableOperation = new Mock<TableOperation>();
//            tableOperation.SetupGet(to => to.Entity).Returns(tableEntity.Object);
//
//            return tableOperation.Object;

            //var tableOperation = new TableOperation(Mock.Of<ITableEntity>(), TableOperationType.Retrieve);
            //make ConfigurationRow non private and use that?
            var tableOperation = TableOperation.Retrieve<ConfigurationRow>("", "");
            
            typeof(TableOperation).GetProperty("Entity").SetValue(tableOperation, tableEntity.Object);

            return tableOperation;
        }
    }

//    public class TestableCloudTable : CloudTable
//    {
//        public TestableCloudTable() : base(new Uri("http://localhost/"))
//        {}
//    }
    
    // why does MS make testing this shit such a pita?
    public class AzureTableStorageConfigurationProviderTestsFixture
    {
        public const string EnvironmentName = "PROD";
        public TestableAzureTableStorageConfigurationProvider ConfigProvider { get; set; }
        public Mock<CloudStorageAccount> CloudStorageAccount { get; set; }
        public Mock<CloudTableClient> CloudTableClient { get; set; }
        //public Mock<CloudTable> CloudTable { get; set; }
//        public Mock<TestableCloudTable> CloudTable { get; set; }
        public Mock<CloudTable> CloudTable { get; set; }
        //public TableResult TableResult { get; set; }

        public AzureTableStorageConfigurationProviderTestsFixture()
        {
            var dummyUri = new Uri("http://example.com/");
            //TableResult = new TableResult();
//            CloudTable = new Mock<TestableCloudTable>();
            CloudTable = new Mock<CloudTable>(dummyUri);
//            CloudTable.Setup(ct => ct.ExecuteAsync(It.IsAny<TableOperation>())).ReturnsAsync(TableResult);
            CloudTableClient = new Mock<CloudTableClient>(dummyUri, new StorageCredentials());
            CloudTableClient.Setup(ctc => ctc.GetTableReference("Configuration")).Returns(CloudTable.Object);
            CloudStorageAccount = new Mock<CloudStorageAccount>(new StorageCredentials(), dummyUri, dummyUri, dummyUri, dummyUri);
            CloudStorageAccount.Setup(csa => csa.CreateCloudTableClient()).Returns(CloudTableClient.Object);
//            ConfigProvider = new TestableAzureTableStorageConfigurationProvider(CloudStorageAccount.Object, EnvironmentName, new[] {""});
        }

        public void SetConfigs(IEnumerable<(string configKey, string json)> configs)
        {
            //do we want this separate?
            ConfigProvider = new TestableAzureTableStorageConfigurationProvider(CloudStorageAccount.Object, EnvironmentName, configs.Select(c => c.configKey));

            //shim static TableOperation.Retrieve
            //TableOperation.Retrieve()
            ConfigProvider.Test_SetConfigs(configs);
            foreach (var config in configs)
            {
//                var configurationRow = new Mock<AzureTableStorageConfigurationProvider.IConfigurationRow>();
//                //configurationRow.SetupGet(te => te.RowKey).Returns(Configs[serviceName]);
//                //configurationRow.SetupGet(te => te.RowKey).Returns(serviceName);
//
//                configurationRow.SetupGet(te => te.Data).Returns(config.json);

                var configurationRow = new AzureTableStorageConfigurationProvider.ConfigurationRow {Data = config.json};

                CloudTable.Setup(ct => ct.ExecuteAsync(It.Is<TableOperation>(to => to.Entity.RowKey == config.configKey))).ReturnsAsync(new TableResult { Result = configurationRow });
            }
        }

        public void Load()
        {
            ConfigProvider.Load();
        }
    }
}