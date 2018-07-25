using Shop.App.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core
{
    public class Engine : IEngine
    {
        public void Run()
        {
            while (true)
            {
                string[] input = Console.ReadLine()
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
