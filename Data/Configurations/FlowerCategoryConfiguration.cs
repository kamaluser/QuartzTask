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
    public class FlowerCategoryConfiguration : IEntityTypeConfiguration<FlowerCategory>
    {
        public void Configure(EntityTypeBuilder<FlowerCategory> builder)
        {
            builder.HasKey(fc => new { fc.FlowerId, fc.CategoryId });

            builder.HasOne(fc => fc.Flower)
                   .WithMany(f => f.FlowerCategories)
                   .HasForeignKey(fc => fc.FlowerId);

            builder.HasOne(fc => fc.Category)
                   .WithMany(c => c.FlowerCategories)
                   .HasForeignKey(fc => fc.CategoryId);
        }
    }
}
