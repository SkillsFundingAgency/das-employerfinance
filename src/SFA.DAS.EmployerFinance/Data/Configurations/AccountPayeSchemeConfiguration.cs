using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data.Configurations
{
    public class AccountPayeSchemeConfiguration : IEntityTypeConfiguration<AccountPayeScheme>
    {
        public void Configure(EntityTypeBuilder<AccountPayeScheme> builder)
        {
            builder.Property(aps => aps.EmployerReferenceNumber).IsRequired().HasColumnType("varchar(16)");
            builder.HasOne(aps => aps.Account).WithMany(a => a.AccountPayeSchemes).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasOne(aps => aps.PayeScheme).WithMany(ps => ps.AccountPayeSchemes).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}