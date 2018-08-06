using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.EntityConfiguration_FluentAPI
{
    class PartCarConfig : IEntityTypeConfiguration<PartCar>
    {
        public void Configure(EntityTypeBuilder<PartCar> builder)
        {
            builder.HasKey(pc => new { pc.Car_Id, pc.Part_Id });

            builder.HasOne(pc => pc.Part)
              .WithMany(p => p.PartCars)
              .HasForeignKey(pc => pc.Part_Id);

            builder.HasOne(pc => pc.Car)
                .WithMany(c => c.PartCars)
                .HasForeignKey(pc => pc.Car_Id);
        }
    }
}
