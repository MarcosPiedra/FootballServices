using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class RefereeConfig : IEntityTypeConfiguration<Referee>
    {
        public void Configure(EntityTypeBuilder<Referee> builder)
        {
            builder.ToTable("Referees");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Name).HasColumnName("Name");
            builder.Property(m => m.MinutesPlayed).HasColumnName("MinutesPlayed");

            builder.HasKey(a => a.Id);
        }
    }
}
