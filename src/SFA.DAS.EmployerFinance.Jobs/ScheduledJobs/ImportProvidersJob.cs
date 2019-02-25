using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Data.DbContextExtensions;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.EmployerFinance.Jobs.ScheduledJobs
{
    public class ImportProvidersJob
    {
        private readonly IProviderApiClient _providerApiClient;
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public ImportProvidersJob(IProviderApiClient providerApiClient, Lazy<EmployerFinanceDbContext> db)
        {
            _providerApiClient = providerApiClient;
            _db = db;
        }

        [Singleton]
        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            const int batchSize = 1000;

            logger.LogInformation("Retrieving all providers from provider API");
            var providers = await _providerApiClient.FindAllAsync();
            logger.LogInformation( $"Retrieved {providers.Count()} providers");
            
            var batches = providers.Batch(batchSize).Select(b => b.ToDataTable(p => p.Ukprn, p => p.ProviderName));

            foreach (var batch in batches)
            {
                logger.LogInformation($"Saving batch of {batch.Rows.Count} to db");
                await _db.Value.ImportProviders(batch);
            }
        }
    }
}