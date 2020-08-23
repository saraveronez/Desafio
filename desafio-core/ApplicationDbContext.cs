using System;
using desafio_core.Mapping;
using desafio_core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace desafio_core
{
     public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#if DEBUG
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddUserSecrets<ApplicationDbContext>();

                var configuration = builder.Build();
                var connectionString = configuration["AppSettings:DefaultConnection"];

                optionsBuilder.UseSqlServer(connectionString);
            }
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMapping());
            modelBuilder.ApplyConfiguration(new DividaMapping());
            modelBuilder.ApplyConfiguration(new ParcelaDividaMapping());
            base.OnModelCreating(modelBuilder);
        }

        #region [DbSet]

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Divida> Divida { get; set; }
        public virtual DbSet<ParcelaDivida> ParcelaDivida { get; set; }

        #endregion
    }
}
