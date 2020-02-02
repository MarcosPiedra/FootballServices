using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class MatchConfig : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Name).HasColumnName("Name");
            builder.Property(m => m.HouseTeamPlayersIds).HasColumnName("HouseTeamPlayersIds");
            builder.Property(m => m.AwayTeamPlayersIds).HasColumnName("AwayTeamPlayersIds");
            builder.Property(m => m.HouseTeamManagerId).HasColumnName("HouseTeamManagerId");
            builder.Property(m => m.AwayTeamManagerId).HasColumnName("AwayTeamManagerId");
            builder.Property(m => m.RefereeId).HasColumnName("RefereeId");
            builder.Property(m => m.Date).HasColumnName("Date")
                                         .HasConversion<string>();
            builder.Property(m => m.Status).HasColumnName("Status")
                                           .HasConversion<int>();
            builder.Property(m => m.IdsIncorrectReported).HasColumnName("IdsIncorrectReported")
                                           .HasConversion<int>();

            

            builder.Ignore(m => m.HouseTeamPlayers);
            builder.Ignore(m => m.AwayTeamPlayers);
            builder.Ignore(m => m.HouseTeamManager);
            builder.Ignore(m => m.AwayTeamManager);
            builder.Ignore(m => m.Referee);

            builder.HasKey(a => a.Id);
        }
    }
}
