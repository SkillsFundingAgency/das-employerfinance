//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using SFA.DAS.EmployerFinance.Models;
//
//namespace SFA.DAS.EmployerFinance.Data.Configurations
//{
//    public class AccountPayeSchemeConfiguration : IEntityTypeConfiguration<AccountPayeScheme>
//    {
//        public void Configure(EntityTypeBuilder<AccountPayeScheme> builder)
//        {
//            builder.Property(aps => aps.EmployerReferenceNumber).IsRequired().HasColumnType("varchar(16)");
//            builder.HasOne(aps => aps.Account).WithMany(a => a.AccountLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
//            builder.HasOne(aps => aps.PayeeScheme).WithMany(ps => ps.AccountLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
//        }
//    }
//}