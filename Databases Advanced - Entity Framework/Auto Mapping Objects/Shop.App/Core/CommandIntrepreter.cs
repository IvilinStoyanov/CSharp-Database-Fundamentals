using Shop.App.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}
