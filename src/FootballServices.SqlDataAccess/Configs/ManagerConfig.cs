using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class ManagerConfig : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("Manager");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Name).HasColumnName("Name");
            builder.Property(m => m.TeamName).HasColumnName("TeamName");
            builder.Property(m => m.YellowCards).HasColumnName("YellowCards");

            builder.HasKey(a => a.Id);
        }
    }
}
