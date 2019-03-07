using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class LevyDeclarationSagaTaskConfiguration : IEntityTypeConfiguration<LevyDeclarationSagaTask>
    {
        public void Configure(EntityTypeBuilder<LevyDeclarationSagaTask> builder)
        {
            builder.Property(s => s.Finished).IsRequired();
        }
    }
}