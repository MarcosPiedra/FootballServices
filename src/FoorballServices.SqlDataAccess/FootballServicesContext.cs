using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoorballServices.SqlDataAccess
{
    public class FootballServicesContext : DbContext
    {
        public FootballServicesContext()
        {
        }

        public DbSet<Manager> Managers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
