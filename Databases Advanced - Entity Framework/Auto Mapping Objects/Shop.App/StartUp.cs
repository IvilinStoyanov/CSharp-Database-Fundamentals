using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using Shop.Services.Contracts;
using Shop.Services;
using Shop.App.Core.Contracts;
using Shop.App.Core;
using Shop.App.Core.Controllers;

namespace Shop.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var service = ConfigureService();

            IEngine engine = new Engine(service);
            engine.Run();
        }

        private static IServiceProvider ConfigureService()
        {
            var serviceColleciton = new ServiceCollection();

            serviceColleciton.AddDbContext<ShopContext>(opts => opts.UseSqlServer(Configuration.ConnectionString));

            serviceColleciton.AddTransient<IDbInitializerService, DbInitializerService>();

            serviceColleciton.AddTransient<ICommandIntrepreter, CommandIntrepreter>();

            serviceColleciton.AddTransient<IEmployeeController, EmployeeController>();

            var serviceProvider = serviceColleciton.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
