using Microsoft.EntityFrameworkCore;
using StockManagement.API.Models;
using StockManagement.API.Models.Entities;

namespace StockManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de índices
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Category);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Price);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Datos semilla para usuarios
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    // Password: admin123 (hashed con BCrypt)
                    PasswordHash = "$2a$11$8K1p/a0dL3LHkKmMhNg7NuLXEzP3L5JGnhXZmXJJKZz0rJXZmXJJK"
                }
            );
        }
    }
}