﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;

namespace Shop.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext()
        { }

        public ShopContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
