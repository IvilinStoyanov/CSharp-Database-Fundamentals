using Shop.App.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.Contracts
{
    public interface IEmployeeController
    {
        void AddEmployee(EmployeeDto employeeDto);

        void SetBirthDay(int employeeId, DateTime date);

        void SetAddress(int employeeId, string address);

        EmployeeDto EmployeeInfo(int employeeId);

        EmployeeInformationDto EmployeePersonalInfo(int employeeId);
    }
}
