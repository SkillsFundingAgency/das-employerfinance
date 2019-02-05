using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;
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
            using (var container = new Container(c =>
            {
                //todo: once DI has been fixed
                //c.AddRegistry<ConfigurationRegistry>();
            }))
            {
                var configuration = container.GetInstance<EmployerFinanceConfiguration>();
                
                using (var connection = new SqlConnection(configuration.DatabaseConnectionString))
                {
                    var optionsBuilder = new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseSqlServer(connection);

                    using (var context = new EmployerFinanceDbContext(optionsBuilder.Options))
                    {
                        var config = new CompareEfSqlConfig
                        {
                            TablesToIgnoreCommaDelimited = "ClientOutboxData,OutboxData"
                        };
                        
                        config.IgnoreTheseErrors("EXTRA IN DATABASE: SFA.DAS.EmployerFinance.Database->Column 'Users', column name. Found = Id");
                        
                        var comparer = new CompareEfSql(config);
                        var hasErrors = comparer.CompareEfWithDb(context);

                        hasErrors.Should().BeFalse(comparer.GetAllErrors);
                    }
                }
            }
        }
    }
}