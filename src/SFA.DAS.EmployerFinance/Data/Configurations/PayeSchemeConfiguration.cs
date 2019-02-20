using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class PayeSchemeConfiguration : IEntityTypeConfiguration<PayeScheme>
    {
        public void Configure(EntityTypeBuilder<PayeScheme> builder)
        {
            builder.HasKey(ps => ps.EmployerReferenceNumber);
            builder.Property(ps => ps.EmployerReferenceNumber).ValueGeneratedNever().IsRequired().HasColumnType("varchar(16)");
            builder.Property(ps => ps.Name).HasColumnType("nvarchar(60)");
        }
    }
}