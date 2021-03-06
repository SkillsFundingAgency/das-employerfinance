﻿using System.Data.Common;
using System.Data.SqlClient;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Data;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<EmployerFinanceConfiguration>().DatabaseConnectionString));
            For<EmployerFinanceDbContext>().Use(c => c.GetInstance<IEmployerFinanceDbContextFactory>().CreateDbContext());
        }
    }
}
