using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.Contracts
{
   public interface ICommand
    {
        string Exucute(string[] args);
    }
}
