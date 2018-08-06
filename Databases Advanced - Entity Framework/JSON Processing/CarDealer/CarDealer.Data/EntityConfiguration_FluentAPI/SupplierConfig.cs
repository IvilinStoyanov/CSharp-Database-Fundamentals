using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.EntityConfiguration_FluentAPI
{
    class SupplierConfig : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired();

            //builder.HasMany(p => p.Parts)
            //    .WithOne(s => s.Supplier)
            //    .HasForeignKey(s => s.Supplier_Id);
        }
    }
}
