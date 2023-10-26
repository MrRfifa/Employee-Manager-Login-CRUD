using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEmployee(Employee employee)
        {
            await _context.AddAsync(employee);
            return await Save();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
            _context.Remove(employee);
            return await Save();
        }

        public async Task<bool> EmployeeExists(int employeeId)
        {
            return await _context.Employees.AnyAsync(e => e.Id == employeeId);
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            var employeeInDb = await _context.Employees.Where(a => a.Id == employeeId).FirstOrDefaultAsync();
            if (employeeInDb is null)
                throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found");
            return employeeInDb;
        }

        public async Task<ICollection<Employee>> GetEmployees()
        {
            return await _context.Employees.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            _context.Update(employee);
            return await Save();
        }
    }
}