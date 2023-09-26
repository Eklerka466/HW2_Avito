using HW2_Avito.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW2_Avito.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Name).HasMaxLength(200);
            builder.HasAlternateKey(user => user.Email);
            builder.Property(user => user.Email).HasMaxLength(100);
            builder.ToTable(table => table.HasCheckConstraint("CK_Age", "\"Age\" > 0 And \"Age\" < 120"));
        }
    }
}