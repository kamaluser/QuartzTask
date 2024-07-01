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
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(s => s.Title).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Desc).HasMaxLength(500);
            builder.Property(s => s.Order).IsRequired();
            builder.Property(s => s.Image).IsRequired();
        }
    }
}
