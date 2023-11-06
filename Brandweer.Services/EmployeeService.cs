using Brandweer.Core; 
using Brandweer.Dto.Results;
using Brandweer.Dto.Requests; 
using Brandweer.Model;
using Brandweer.Services.Extensions; 
using Microsoft.EntityFrameworkCore;
using Brandweer.Services.Model;
using Brandweer.Services.Model.Extensions;

namespace Brandweer.Services
{
    public class EmployeeService
    {
        private readonly BrandweerDbContext _dbContext;

        public EmployeeService(BrandweerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CRUD Employee
        //Find
        public async Task<IList<EmployeeResult>> FindAsync()
        {
            return await _dbContext.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ProjectToEmployeeResults()
                .ToListAsync();
        }

        //Get by id
        public async Task<EmployeeResult?> GetAsync(int id)
        {
            return await _dbContext.Employees
                .ProjectToEmployeeResults()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        //Create
        public async Task<ServiceResult<EmployeeResult?>> CreateAsync(EmployeeRequest request)
        {
          
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            _dbContext.Employees.Add(employee);

            await _dbContext.SaveChangesAsync();

            var employeeResult = await GetAsync(employee.Id);

            return new ServiceResult<EmployeeResult?>(employeeResult);
        }

        //Update
        public async Task<ServiceResult<EmployeeResult?>> UpdateAsync(int id, EmployeeRequest request)
        {
            var dbEmployee = await _dbContext.Employees.FindAsync(id);
            if (dbEmployee is null)
            {
                return new ServiceResult<EmployeeResult?>().NotFound("employee");
            }

            dbEmployee.FirstName = request.FirstName;
            dbEmployee.LastName = request.LastName;
            // Other properties as needed

            await _dbContext.SaveChangesAsync();

            var employeeResult = await GetAsync(id);
            return new ServiceResult<EmployeeResult?>(employeeResult);
        }

        //Delete
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee is null)
            {
                return new ServiceResult().NotFound("employee");
            }

            _dbContext.Employees.Remove(employee);

            await _dbContext.SaveChangesAsync();

            return new ServiceResult();
        }
    }
}
