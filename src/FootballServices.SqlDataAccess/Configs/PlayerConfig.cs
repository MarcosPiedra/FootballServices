using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class PlayerConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Name).HasColumnName("Name");
            builder.Property(m => m.Number).HasColumnName("Number");
            builder.Property(m => m.TeamName).HasColumnName("TeamName");
            builder.Property(m => m.YellowCards).HasColumnName("YellowCards");
            builder.Property(m => m.RedCards).HasColumnName("RedCards");
            builder.Property(m => m.MinutesPlayed).HasColumnName("MinutesPlayed");

            builder.HasKey(a => a.Id);
        }
    }
}
