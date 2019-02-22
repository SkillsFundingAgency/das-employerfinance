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

        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            var providers = await _providerApiClient.FindAllAsync();
            var batches = providers.Batch(1000).Select(b => b.ToDataTable(p => p.Ukprn, p => p.ProviderName));

            foreach (var batch in batches)
            {
                await _db.Value.ImportProviders(batch);
            }
        }
    }
}