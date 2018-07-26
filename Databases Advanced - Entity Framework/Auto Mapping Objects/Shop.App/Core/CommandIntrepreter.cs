﻿using Shop.App.Core.Contracts;

using System;
using System.Linq;
using System.Reflection;

namespace Shop.App.Core
{
    public class CommandIntrepreter : ICommandIntrepreter
    {
        private readonly IServiceProvider serviceProvider;

        public CommandIntrepreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] input)
        {
            string commandName = input[0] + "Command";

            string[] args = input.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                               .GetTypes()
                               .FirstOrDefault(x => x.Name == commandName);

            var constructor = type
                              .GetConstructors()
                              .First();

            var constructorParams = constructor
                                    .GetParameters()
                                    .Select(s => s.ParameterType)
                                    .ToArray();

            var service = constructorParams
                                    .Select(serviceProvider.GetService)
                                    .ToArray();

            var command = (ICommand)constructor.Invoke(service);

            var result = command.Exucute(args);

            return result;
        }
    }
}
