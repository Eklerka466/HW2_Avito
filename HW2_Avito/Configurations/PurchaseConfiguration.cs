using HW2_Avito.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW2_Avito.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(purchase => purchase.Id);
            builder.HasOne(user => user.User).WithMany(purchase => purchase.Purchases).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(goods => goods.Goods).WithMany(purchase => purchase.Purchases).OnDelete(DeleteBehavior.Cascade);
        }
    }
}