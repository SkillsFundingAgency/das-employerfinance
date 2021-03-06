using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class LevyDeclarationSagaConfiguration : IEntityTypeConfiguration<LevyDeclarationSaga>
    {
        public void Configure(EntityTypeBuilder<LevyDeclarationSaga> builder)
        {
            builder.Property(s => s.Updated).IsConcurrencyToken();
        }
    }
}