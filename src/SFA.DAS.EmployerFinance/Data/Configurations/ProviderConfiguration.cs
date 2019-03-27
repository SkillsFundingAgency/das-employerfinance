using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(p => p.Ukprn);
            builder.Property(p => p.Ukprn).ValueGeneratedNever();
            builder.Property(p => p.Name).IsRequired().HasColumnType("nvarchar(100)");
        }
    }
}