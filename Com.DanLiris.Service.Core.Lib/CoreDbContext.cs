using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Com.DanLiris.Service.Core.Lib.Models;

namespace Com.DanLiris.Service.Core.Lib
{
    public class CoreDbContext : BaseDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public DbSet<AccountBank> AccountBanks { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<GarmentCurrency> GarmentCurrencies { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Storage> Storages { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<TermOfPayment> TermOfPayments { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Uom> UnitOfMeasurements { get; set; }

        public DbSet<Vat> Vats { get; set; }

        public DbSet<Quality> Qualities { get; set; }
        public DbSet<Comodity> Comodities { get; set; }
        public DbSet<YarnMaterial> YarnMaterials { get; set; }
        public DbSet<MaterialConstruction> MaterialConstructions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Buyer>()
                .HasIndex(b => b.Code);
        }
    }
}
