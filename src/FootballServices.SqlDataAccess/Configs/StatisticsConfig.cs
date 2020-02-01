using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class StatisticsConfig : IEntityTypeConfiguration<Statistic>
    {
        public void Configure(EntityTypeBuilder<Statistic> builder)
        {
            builder.ToTable("Statistics");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Total).HasColumnName("Total");
            builder.Property(m => m.TeamName).HasColumnName("TeamName");
            builder.Property(m => m.Type).HasColumnName("Type")
                                           .HasConversion<int>(); 

            builder.HasKey(a => a.Id);
        }
    }
}
