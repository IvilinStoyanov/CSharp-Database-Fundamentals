using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace P02_DatabaseFirst.Data
{
    public class Engine
    {
        public void Run()
        {
            // this.EmployeesFullInformation(); 
            // this.EmployeesWithSalaryOver50000();
            // this.EmployeesFromResearchAndDevelopment();
            // this.AddingNewAddressAndUpdatingEmployee();
            // this.EmployeesAndProjects();
            // this.AddressesByTown();
            // this.Employee(147)
            // this.FindDepartmentsWithMoreThanFiveEmployees(5);
            // this.FindLatestTenProjects(10);
            // this.IncreaseSalaries(12, "Engineering", "Tool Design", "Marketing", "Information Services");
            // this.FindFirstNameStartingWithSa("Sa");
            this.DeleteProjectById(2);

        }

        // P03
        private void EmployeesFullInformation()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context
                    .Employees
                    .OrderBy(x => x.EmployeeId)
                    .Select(x => new
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MiddleName = x.MiddleName,
                        JobTitle = x.JobTitle,
                        Salary = x.Salary
                    });

                using (StreamWriter sw =
                    new StreamWriter("../../../../P03_EmployeesFullInformation.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
                    }
                }
            }
        }

        // P04
        private void EmployeesWithSalaryOver50000()
        {
            var dbContext = new SoftUniContext();

            using (dbContext)
            {
                var employeeNames = dbContext
                    .Employees
                    .Where(e => e.Salary > 50000)
                    .OrderBy(e => e.FirstName)
                    .Select(e => e.FirstName);

                using (StreamWriter sw =
                    new StreamWriter("../../../../P04_EmployeesWithSalaryOver50000.txt"))
                    foreach (var name in employeeNames)
                    {
                        sw.WriteLine($"{name}");
                    }
            }
        }

        // P05
        private void EmployeesFromResearchAndDevelopment()
        {
            var dbContext = new SoftUniContext();

            using (dbContext)
            {
                var employees = dbContext
                    .Employees
                    .Where(e => e.Department.Name == "Research and Development")
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        DepartmentName = e.Department.Name,
                        Salary = e.Salary
                    });
                using (StreamWriter sw =
                   new StreamWriter("../../../../P05_EmployeesFromResearchAndDevelopment.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
                    }
                }
            }
        }

        // P06
        private void AddingNewAddressAndUpdatingEmployee()
        {
            var dbContext = new SoftUniContext();

            using (dbContext)
            {
                //Address address = new Address()
                //{
                //    AddressText = "Vitoshka 15",
                //    TownId = 4
                //};

                //var name = dbContext.Employees.FirstOrDefault(e => e.LastName == "Nakov");
                //name.Address = address;

                var adresses = dbContext
                    .Employees
                    .OrderByDescending(x => x.AddressId)
                    .Select(e => e.Address.AddressText)
                    .Take(10)
                    .ToArray();

                dbContext.SaveChanges();

                foreach (var a in adresses)
                {
                    Console.WriteLine(a);
                }
            }
        }

        // P07 
        private void EmployeesAndProjects()
        {
            using (var context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.EmployeesProjects
                        .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2013))
                    .Take(30)
                    .Select(e => new
                    {
                        Employee = $"{e.FirstName} {e.LastName}",
                        Manager = $"{e.Manager.FirstName} {e.Manager.LastName}",
                        Projects = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ep.Project.Name,
                                ep.Project.StartDate,
                                ep.Project.EndDate
                            })
                    });

                foreach (var emp in employees)
                {
                    Console.WriteLine($"{emp.Employee} - Manager: {emp.Manager}");

                    foreach (var project in emp.Projects)
                    {
                        var endDate = project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished";
                        Console.WriteLine($"--{project.Name} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                    }
                }
            }

        }

        // P08
        private void AddressesByTown()
        {
            using (var context = new SoftUniContext())
            {


                var addresses = context.Addresses
                    .Select(a => new
                    {
                        AddressText = a.AddressText,
                        TownName = a.Town.Name,
                        CountEmployee = a.Employees.Count
                    })
                    .OrderByDescending(a => a.CountEmployee)
                    .ThenBy(a => a.TownName)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToArray();

                foreach (var a in addresses)
                {
                    Console.WriteLine($"{a.AddressText}, {a.TownName} - {a.CountEmployee} employees");
                }

            }
        }

        // P09
        private void Employee(int id)
        {
            using (var context = new SoftUniContext())
            {
                var employee = context.Employees
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        Projects = e.EmployeesProjects
                            .Select(ep => ep.Project.Name)
                            .OrderBy(pn => pn)
                            .ToArray()
                    })
                    .FirstOrDefault(e => e.EmployeeId == id);

                if (employee != null)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                    Console.WriteLine(string.Join(Environment.NewLine, employee.Projects));
                }
            }
        }

        // P10
        private void FindDepartmentsWithMoreThanFiveEmployees(int numberOfEmployees)
        {
            using (var context = new SoftUniContext())
            {
                var departmentSeparator = $"{Environment.NewLine}{new string('-', 10)}{Environment.NewLine}";

                Console.WriteLine(string.Join(departmentSeparator, context.Departments
                    .Where(d => d.Employees.Count > numberOfEmployees)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => $"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}{Environment.NewLine}" +
                        $@"{string.Join(Environment.NewLine, d.Employees
                            .OrderBy(e => e.FirstName)
                            .ThenBy(e => e.LastName)
                            .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle}"))}")));

                Console.WriteLine(new string('-', 10));
            }
        }

        // P11
        private void FindLatestTenProjects(int countProjects)
        {
            using (var context = new SoftUniContext())
            {
                var projects =
                    context.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(countProjects)
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name}{Environment.NewLine}{p.Description}{Environment.NewLine}{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");

                Console.WriteLine(string.Join(Environment.NewLine, projects));
            }
        }

        // P12
        private void IncreaseSalaries(decimal percentage, params string[] departments)
        {
            percentage /= 100.0M;

            using (var context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => departments
                        .Any(d => d.Equals(e.Department.Name, StringComparison.OrdinalIgnoreCase)))
                    .Distinct()
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

                foreach (var e in employees)
                {
                    e.Salary *= (1 + percentage);
                    Console.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
                }

                context.SaveChanges();
            }

        }

        // P13 
        private void FindFirstNameStartingWithSa(string name)
        {
            using (var context = new SoftUniContext())
            {
                Console.WriteLine(string.Join(Environment.NewLine, context.Employees
                    .Where(e => e.FirstName.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})")));
            }
        }

        // P14
        private void DeleteProjectById(int id)
        {
            using (var context = new SoftUniContext())
            {
                var project = context.Projects.Find(id);
                if (project != null)
                {
                    context.EmployeesProjects.RemoveRange(context.EmployeesProjects
                        .Where(ep => ep.Project == project));

                    context.Projects.Remove(project);
                    context.SaveChanges();
                }

                Console.WriteLine(string.Join(Environment.NewLine, context.Projects
                    .OrderBy(p => p.ProjectId)
                    .Take(10)
                    .Select(p => p.Name)));
            }

        }
    }
}
