using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services.Contracts;
using System;

namespace Shop.Services
{
    public class DbInitializerService : IDbInitializerService
    {
        private readonly ShopContext context;

        public DbInitializerService(ShopContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}
