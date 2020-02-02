using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.SqlDataAccess.Configs
{
    public class NotValidBeforeStartConfig : IEntityTypeConfiguration<NotValidBeforeStart>
    {
        public void Configure(EntityTypeBuilder<NotValidBeforeStart> builder)
        {
            builder.ToTable("NotValidBeforeStart");

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.RelatedId).HasColumnName("RelatedId");
            builder.Property(m => m.MatchId).HasColumnName("MatchId");
            builder.Property(m => m.RelatedType).HasColumnName("RelatedTypeId")
                                         .HasConversion<int>(); 

            builder.HasKey(a => a.Id);
        }
    }
}
