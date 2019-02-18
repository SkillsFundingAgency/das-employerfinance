using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class ProcessLevyDeclarationsJobConfiguration : IEntityTypeConfiguration<ProcessLevyDeclarationsJob>
    {
        public void Configure(EntityTypeBuilder<ProcessLevyDeclarationsJob> builder)
        {
            builder.Property(j => j.Id).ValueGeneratedNever();
            builder.Property(j => j.Updated).IsConcurrencyToken();
        }
    }
}