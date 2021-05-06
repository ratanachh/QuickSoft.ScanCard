using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickSoft.ScanCard.Domain;

namespace QuickSoft.ScanCard.Infrastructure.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .HasOne<Card>(customer => customer.Card)
                .WithOne(card => card.Customer)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}