using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Models;

namespace PharmacyManagement.Data
{
    // Inherit from IdentityDbContext to manage Users, Roles, and Claims
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MUST call base.OnModelCreating(modelBuilder) for Identity tables to be created
            base.OnModelCreating(modelBuilder);

            // Configure Decimal Precisions
            modelBuilder.Entity<Purchase>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PurchaseItem>().Property(pi => pi.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PurchaseItem>().Property(pi => pi.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Stock>().Property(s => s.PurchasePrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Stock>().Property(s => s.SellingPrice).HasColumnType("decimal(18,2)");

            // Relationships
            modelBuilder.Entity<Medicine>().HasOne(m => m.Category).WithMany(c => c.Medicines).HasForeignKey(m => m.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne(p => p.Customer).WithMany(c => c.Purchases).HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}