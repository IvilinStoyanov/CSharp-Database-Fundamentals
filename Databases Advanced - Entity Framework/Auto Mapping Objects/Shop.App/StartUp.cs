using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using Shop.Services.Contracts;
using Shop.Services;

namespace Shop.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var service = ConfigureService();
        }

        private static IServiceProvider ConfigureService()
        {
            var serviceColleciton = new ServiceCollection();

            serviceColleciton.AddDbContext<ShopContext>(opts => opts.UseSqlServer(Configuration.ConnectionString));

            serviceColleciton.AddTransient<IDbInitializerService, DbInitializerService>();

            var serviceProvider = serviceColleciton.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
