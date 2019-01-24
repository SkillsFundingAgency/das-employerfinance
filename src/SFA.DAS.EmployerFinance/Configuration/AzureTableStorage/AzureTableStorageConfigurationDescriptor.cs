using System;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    public class AzureTableStorageConfigurationDescriptor
    {
        public AzureTableStorageConfigurationDescriptor(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}