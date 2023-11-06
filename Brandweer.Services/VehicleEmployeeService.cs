using Brandweer.Core;
using Brandweer.Dto.Requests;
using Brandweer.Model;
using Brandweer.Services.Model;
using Microsoft.EntityFrameworkCore;

namespace Brandweer.Services
{
    public class VehicleEmployeeService
    {
        private readonly BrandweerDbContext _dbContext;

        public VehicleEmployeeService(BrandweerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> AssignEmployeeToVehicleAsync(VehicleEmployeeRequest request)
        {
            var result = new ServiceResult();
            var existingAssignment = await _dbContext.VehicleEmployees
                .AnyAsync(ve => ve.VehicleId == request.VehicleId && ve.EmployeeId == request.EmployeeId);

            if (existingAssignment)
            {
                result.Messages.Add(new ServiceMessage
                {
                    Code = "AlreadyAssigned",
                    Title = "Employee is already assigned to the vehicle.",
                    Type = ServiceMessageType.Error
                });
            }
            else
            {
                var vehicleEmployee = new VehicleEmployee
                {
                    VehicleId = request.VehicleId,
                    EmployeeId = request.EmployeeId
                };

                _dbContext.VehicleEmployees.Add(vehicleEmployee);
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<ServiceResult> RemoveEmployeeFromVehicleAsync(int vehicleId, int employeeId)
        {
            var result = new ServiceResult();
            var vehicleEmployee = await _dbContext.VehicleEmployees
                .FirstOrDefaultAsync(ve => ve.VehicleId == vehicleId && ve.EmployeeId == employeeId);

            if (vehicleEmployee == null)
            {
                result.Messages.Add(new ServiceMessage
                {
                    Code = "NotAssigned",
                    Title = "Employee is not assigned to the vehicle.",
                    Type = ServiceMessageType.Error
                });
            }
            else
            {
                _dbContext.VehicleEmployees.Remove(vehicleEmployee);
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}
