using Shop.App.Core.Contracts;
using Shop.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var initializeDb = this.serviceProvider.GetService<IDbInitializerService>();
            initializeDb.InitializeDatabase();

            var commandIntrepreter = this.serviceProvider.GetService<ICommandIntrepreter>();


            while (true)
            {
                string[] input = Console.ReadLine()
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var result = commandIntrepreter.Read(input);
                Console.WriteLine(result);
            }
        }
    }
}
