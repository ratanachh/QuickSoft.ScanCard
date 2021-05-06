using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickSoft.ScanCard.Domain;

namespace QuickSoft.ScanCard.Infrastructure.EntityConfigurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder
                .HasOne<Person>(p => p.Person)
                .WithMany(s => s.Audits)
                .HasForeignKey(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne<Card>(a => a.Card)
                .WithMany(c => c.Audit)
                .HasForeignKey(a => a.CardId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .Property(c => c.Descriptions)
                .UseCollation(CollationConfiguration.Khmer_100_BIN);
        }
    }
}