using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickSoft.ScanCard.Domain;

namespace QuickSoft.ScanCard.Infrastructure.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder
                .Property(c => c.Name)
                .UseCollation(CollationConfiguration.Khmer_100_BIN);
        }
    }
}