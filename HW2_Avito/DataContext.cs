using HW2_Avito.Entities;
using HW2_Avito.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HW2_Avito
{
    internal class DataContext : DbContext
    {
        //public readonly static StreamWriter logStream = new StreamWriter("C:\\Users\\Valeriya\\source\\repos\\HW2_Avito\\HW2_Avito\\LogFile.txt", true);

        public DbSet<User> Users { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Avito;Username=postgres;Password=1234");
            optionsBuilder.LogTo(Logger.GetLogger().WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
        }

        public DataContext()
        {
            Database.EnsureCreated();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}