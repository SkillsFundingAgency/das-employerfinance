using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Jobs.ScheduledJobs;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Jobs.ScheduledJobs
{
    [TestFixture]
    [Parallelizable]
    public class ImportProvidersJobTests : FluentTest<ImportProvidersJobTestsFixture>
    {
        [Test]
        public Task Run_WhenRunningImportProvidersJob_ThenShouldImportProvidersInBatchesOf1000()
        {
            return TestAsync(f => f.SetProviders(1500), f => f.Run(), f => f.Db.Verify(d => d.ExecuteSqlCommandAsync(
                "EXEC ImportProviders @providers, @now",
                It.Is<SqlParameter>(p => p.ParameterName == "providers"),
                It.Is<SqlParameter>(p => p.ParameterName == "now" && (DateTime)p.Value >= f.Now)), Times.Exactly(2)));
        }
        
        [Test]
        public Task Run_WhenRunningImportProvidersJob_ThenShouldImportProviders()
        {
            return TestAsync(f => f.SetProviders(1500), f => f.Run(), f => f.ImportedProviders.Should().BeEquivalentTo(f.Providers));
        }
    }

    public class ImportProvidersJobTestsFixture
    {
        public DateTime Now { get; set; }
        public Mock<EmployerFinanceDbContext> Db { get; set; }
        public ImportProvidersJob Job { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }
        public List<ProviderSummary> Providers { get; set; }
        public List<ProviderSummary> ImportedProviders { get; set; }

        public ImportProvidersJobTestsFixture()
        {
            Now = DateTime.UtcNow;
            Db = new Mock<EmployerFinanceDbContext>();
            ProviderApiClient = new Mock<IProviderApiClient>();
            ImportedProviders = new List<ProviderSummary>();
            
            Db.Setup(d => d.ExecuteSqlCommandAsync(It.IsAny<string>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>()))
                .Returns(Task.CompletedTask)
                .Callback<string, object[]>((s, p) =>
                {
                    var sqlParameter = (SqlParameter)p[0];
                    var dataTable = (DataTable)sqlParameter.Value;
                    
                    ImportedProviders.AddRange(dataTable.Rows.Cast<DataRow>().Select(r => new ProviderSummary
                    {
                        Ukprn = (long)r[0],
                        ProviderName = (string)r[1]
                    }));
                });
            
            Job = new ImportProvidersJob(ProviderApiClient.Object, new Lazy<EmployerFinanceDbContext>(() => Db.Object));
        }

        public Task Run()
        {
            return Job.Run(null, null);
        }

        public ImportProvidersJobTestsFixture SetProviders(int count)
        {
            Providers = Enumerable.Range(1, count)
                .Select(i => new ProviderSummary
                {
                    Ukprn = i,
                    ProviderName = i.ToString()
                })
                .ToList();
            
            ProviderApiClient.Setup(c => c.FindAllAsync()).ReturnsAsync(Providers);
            
            return this;
        }
    }
}