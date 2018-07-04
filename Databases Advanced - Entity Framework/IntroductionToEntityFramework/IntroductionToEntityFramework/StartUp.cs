using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P02_DatabaseFirst
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employee = context.Employees.ToArray();

                foreach (var e in employee)
                {
                    Console.WriteLine(e.FirstName);
                }
            }
        }
    }
}
