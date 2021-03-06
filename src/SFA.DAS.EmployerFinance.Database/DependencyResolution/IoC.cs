﻿using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Database.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
