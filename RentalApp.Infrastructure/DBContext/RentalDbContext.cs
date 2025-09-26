using Microsoft.EntityFrameworkCore;
using RentalApp.Core.Entities;
using RentalApp.Infrastructure.Entities;

namespace RentalApp.Infrastructure.DBContext
{
    public class RentalDBContext(DbContextOptions<RentalDBContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Motorcycle>().HasIndex(m => m.VehiclePlate).IsUnique();
            modelBuilder.Entity<Renter>().HasIndex(r => r.CompanyRegistrationNumber).IsUnique();
            modelBuilder.Entity<Renter>().HasIndex(r => r.DriverLicenseNumber).IsUnique();

            modelBuilder.Entity<Motorcycle2024Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.HasIndex(e => e.MotorcycleId);
                entity.HasIndex(e => e.CreatedAt);
            });
                        
            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
        public DbSet<Renter> Renters => Set<Renter>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<Motorcycle2024Notification> Motorcycle2024Notifications { get; set; }

        // ⚡ Override SaveChanges para atualizar UpdatedAt sempre
        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Motorcycle &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((Motorcycle)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                ((Motorcycle)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
