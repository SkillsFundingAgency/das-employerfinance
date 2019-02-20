using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Types.Configuration;
using TestSupport.EfSchemeCompare;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    /// <summary>
    /// https://www.thereformedprogrammer.net/ef-core-taking-full-control-of-the-database-schema/
    /// </summary>
    [TestFixture]
    public class EntityFrameworkSchemaCheckTests
    {
        [Test]
        [Ignore("To be run adhoc (but could live in an integration test)")]
        public void CheckDatabaseSchemaAgainstEntityFrameworkExpectedSchema()
        {
            var configuration = new ConfigurationBuilder().AddAzureTableStorage(EmployerFinanceConfigurationKeys.Base).Build();
            var employerFinanceConfiguration = configuration.GetEmployerFinanceSection<EmployerFinanceConfiguration>();
            
            using (var connection = new SqlConnection(employerFinanceConfiguration.DatabaseConnectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseSqlServer(connection);

                using (var context = new EmployerFinanceDbContext(optionsBuilder.Options))
                {
                    var config = new CompareEfSqlConfig
                    {
                        TablesToIgnoreCommaDelimited = "ClientOutboxData,OutboxData,SchemaVersions"
                    };
                    
                    config.IgnoreTheseErrors(
                        "EXTRA IN DATABASE: SFA.DAS.EmployerFinance.Database->Column 'Users', column name. Found = Id\n" +
                        "EXTRA IN DATABASE: SFA.DAS.EmployerFinanceV2.Database->Column 'PayeSchemes', column name. Found = Id");
                    
                    var comparer = new CompareEfSql(config);
                    var hasErrors = comparer.CompareEfWithDb(context);

                    hasErrors.Should().BeFalse(comparer.GetAllErrors);
                }
            }
        }
    }
}