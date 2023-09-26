using HW2_Avito.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW2_Avito.Configurations
{
    public class GoodsConfiguration : IEntityTypeConfiguration<Goods>
    {
        public void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.HasKey(goods => goods.Id);
            builder.Property(goods => goods.Id).HasIdentityOptions(1);
            builder.Property(goods => goods.Name).HasMaxLength(100);
            builder.Property(goods => goods.Description).HasMaxLength(500);
            builder.Property(goods => goods.Price).HasPrecision(10, 2);
            builder.ToTable(t => t.HasCheckConstraint("CK_Price", "\"Price\" > 0"));
        }
    }
}