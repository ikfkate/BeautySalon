using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class BeautySalonContext : DbContext
    {
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        // Настройка подключения
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Путь к appsettings.json 
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                IConfiguration configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                // Подключаем PostgreSQL
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        // Настройка моделей 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка точности для денег (Price)
            modelBuilder.Entity<Procedure>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Procedure>()
                .ToTable("procedures", "public");
        }
    }
}