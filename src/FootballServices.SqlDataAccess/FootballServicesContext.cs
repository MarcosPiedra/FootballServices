using FootballServices.Domain.Models;
using FootballServices.SqlDataAccess.Configs;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoorballServices.SqlDataAccess
{
    public class FootballServicesContext : DbContext
    {
        public FootballServicesContext() : base()
        {
        }

        public FootballServicesContext(DbContextOptions<FootballServicesContext> options) : base(options)
        {
        }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Referee> Referee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ManagerConfig());
            modelBuilder.ApplyConfiguration(new PlayerConfig());
            modelBuilder.ApplyConfiguration(new RefereeConfig());
        }
    }
}
