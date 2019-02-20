using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class PayeSchemeConfiguration : IEntityTypeConfiguration<PayeScheme>
    {
        public void Configure(EntityTypeBuilder<PayeScheme> builder)
        {
            builder.HasKey(p => p.EmployerReferenceNumber);
            builder.Property(p => p.EmployerReferenceNumber).ValueGeneratedNever().IsRequired().HasColumnType("varchar(16)");
            builder.Property(p => p.Name).HasColumnType("nvarchar(60)");
        }
    }
}