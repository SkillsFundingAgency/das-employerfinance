using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class AccountPayeSchemeConfiguration : IEntityTypeConfiguration<AccountPayeScheme>
    {
        public void Configure(EntityTypeBuilder<AccountPayeScheme> builder)
        {
//            builder.HasKey(p => p.Id);
//            builder.Property(p => p.EmployerReferenceNumber).ValueGeneratedNever();
            builder.Property(p => p.EmployerReferenceNumber).IsRequired().HasColumnType("varchar(16)");
        }
    }
}