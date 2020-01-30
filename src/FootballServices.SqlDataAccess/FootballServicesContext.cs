using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoorballServices.SqlDataAccess
{
    public class FootballServicesContext : DbContext
    {
        private string path;

        public FootballServicesContext(string path)
        {
            this.path = path;
        }

        public DbSet<Manager> Managers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MapNode>();
            //modelBuilder.Entity<MapPoint>();
            //modelBuilder.Entity<MapSegment>();

            //modelBuilder.Entity<Items>(entity =>
            //{
            //    entity.HasOne(d => d.MapNode)
            //    .WithMany(p => p.Items)
            //    .HasForeignKey(d => d.ParentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(d => d.MapPoint)
            //    .WithMany(p => p.Items)
            //    .HasForeignKey(d => d.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(d => d.MapSegment)
            //    .WithMany(p => p.Items)
            //    .HasForeignKey(d => d.Id)
            //    .OnDelete(DeleteBehavior.Cascade);
            //});

            base.OnModelCreating(modelBuilder);
        }
    }
}
