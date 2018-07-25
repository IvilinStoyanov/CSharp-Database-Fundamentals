using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shop.App.Core.Contracts;
using Shop.App.Core.DTOs;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Controllers
{
    public class EmployeeController : IEmployeeController
    {
        private readonly ShopContext context;

        public EmployeeController(ShopContext context)
        {
            this.context = context;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            var employee = Mapper.Map<Employee>(employeeDto);

            this.context.Employees.Add(employee);

            this.context.SaveChanges();
        }

        public void SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            if(employee == null)
            {
                throw new ArgumentException("Invalid Id");
            }

            employee.Address = address;

            this.context.SaveChanges(); 
        }

        public void SetBirthDay(int employeeId, DateTime date)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid Id");
            }

            employee.Birthday = date;

            this.context.SaveChanges();
        }

        public EmployeeDto EmployeeInfo(int employeeId)
        {
            var employee = context.Employees
                                  .Where(x => x.Id == employeeId)
                                  .ProjectTo<EmployeeDto>()
                                  .SingleOrDefault();

            if(employee == null)
            {
                throw new ArgumentException("Invalid Id");
            }

            return employee;
        }

        public EmployeeInformationDto EmployeePersonalInfo(int employeeId)
        {
            var employee = context.Employees
                                  .Where(x => x.Id == employeeId)
                                  .ProjectTo<EmployeeInformationDto>()
                                  .SingleOrDefault();

            if (employee == null)
            {
                throw new ArgumentException("Invalid Id");
            }

            return employee;
        }
    }
}
