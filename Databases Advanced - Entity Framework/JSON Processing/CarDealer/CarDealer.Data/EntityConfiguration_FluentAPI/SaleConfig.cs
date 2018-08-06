using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.EntityConfiguration_FluentAPI
{
    class SaleConfig : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(c => c.Car)
                .WithMany(s => s.Sales)
                .HasForeignKey(c => c.Car_Id);

            builder.HasOne(c => c.Customer)
                .WithMany(s => s.Sales)
                .HasForeignKey(c => c.Customer_Id);
        }
    }
}
