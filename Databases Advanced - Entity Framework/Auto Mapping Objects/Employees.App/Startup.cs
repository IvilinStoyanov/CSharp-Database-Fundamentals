namespace Employees.App
{
    using AutoMapper;
    using Core;
    using Employees.App.Interfaces;
    using Employees.App.IO;
    using Employees.Data;
    using Employees.Data.Config;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Startup
    {
        public static void Main()
        {
            InitializeMapper();

            var reader = new ConsoleReader();
            var writer = new ConsoleWriter();
            var serviceProvider = ConfigureServices();
            var commandInterpreter = new CommandInterpreter<ICommand>(serviceProvider);

            var engine = new Engine(reader, writer, commandInterpreter);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(options =>
                options.UseSqlServer(DbConfig.ConnectionString)
            );

            serviceCollection.AddTransient<IWriter, ConsoleWriter>();
            serviceCollection.AddTransient<IReader, ConsoleReader>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<MapperProfile>());
        }
    }
}
