using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int employeeId);
        Task<bool> EmployeeExists(int employeeId);
        Task<bool> CreateEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(Employee employee);
        Task<bool> Save();
    }
}