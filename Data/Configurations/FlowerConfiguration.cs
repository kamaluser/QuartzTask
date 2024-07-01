using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class FlowerConfiguration : IEntityTypeConfiguration<Flower>
    {
        public void Configure(EntityTypeBuilder<Flower> builder)
        {
            builder.Property(f => f.Name).IsRequired().HasMaxLength(25);
            builder.Property(f => f.Desc).HasMaxLength(500);
            builder.Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasMany(f => f.Photos)
                   .WithOne(p => p.Flower)
                   .HasForeignKey(p => p.FlowerId);

            builder.HasMany(f => f.FlowerCategories)
                   .WithOne(fc => fc.Flower)
                   .HasForeignKey(fc => fc.FlowerId);
        }
    }
}
