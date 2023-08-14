﻿using HRM.API.Db;
using HRM.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRM.API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMContext _context;

        public EmployeeRepository(HRMContext context)
        {
            _context = context;
        }

        public async Task<string> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return "Data not found!";
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return "Data deleted successfully!";
        }

        public async Task<string> SaveEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            try
            {
                await _context.SaveChangesAsync();
                return "Data saved successfully!";
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<IEnumerable<Employee>> SelectAllEmployees()
        {
            try
            {
                var allEmployees = _context.Employees.ToListAsync();
                return await allEmployees;
            }catch(Exception exp)
            {
                throw (exp);
            }
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        public async Task<Employee> SelectEmployee(int id)
        {
            try
            {
                var employee = _context.Employees.FindAsync(id);
                return await employee;
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<string> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return "Cannot be updated!";
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return "Data updated successfully!";
            }
            catch (DbUpdateConcurrencyException exp)
            {
                if (!EmployeeExists(id))
                {
                    return "Data not found!";
                }
                else
                {
                    throw (exp);
                }
            }
        }
    }
}
