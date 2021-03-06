﻿using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<ApprenticeshipInfoServiceApiRegistry>();
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<DateTimeRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}