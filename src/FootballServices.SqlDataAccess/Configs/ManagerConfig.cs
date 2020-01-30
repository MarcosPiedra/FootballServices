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
            //builder.Property(a => a.Id).HasColumnName("ac_id");
            //builder.Property(a => a.ActionName).HasColumnName("ac_name");
            //builder.Property(a => a.MacroId).HasColumnName("ac_macro_id");
            //builder.Property(a => a.ActionType).HasColumnName("ac_type");
            //builder.Property(a => a.Trigger).HasColumnName("ac_trigger");
            //builder.Property(a => a.Distance).HasColumnName("ac_distance");
            //builder.Property(a => a.TagId).HasColumnName("ac_tag_id");
            //builder.Property(a => a.Function).HasColumnName("ac_function");
            //builder.Property(a => a.FunctionValue).HasColumnName("ac_value");
            //builder.Property(a => a.Notes).HasColumnName("ac_notes");

            //builder.HasOne(a => a.Tag)
            //       .WithOne(t => t.ActionMacro)
            //       .HasForeignKey<ActionMacro>(a => a.TagId);

            //builder.Ignore(a => a.InternalGuid);

            //builder.HasKey(a => a.Id);
            builder.ToTable("action");
        }
    }
}
