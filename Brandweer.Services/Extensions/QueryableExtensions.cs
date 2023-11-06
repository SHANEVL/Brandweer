using Brandweer.Dto.Results;
using Brandweer.Model;
using System.Linq;

namespace Brandweer.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<EmployeeResult> ProjectToEmployeeResults(this IQueryable<Employee> query)
        {
            return query.Select(e => new EmployeeResult
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            });
        }

        public static IQueryable<VehicleResult> ProjectToVehicleResults(this IQueryable<Vehicle> query)
        {
            return query.Select(v => new VehicleResult
            {
                Id = v.Id,
                Description = v.Description,
                Capacity = v.Capacity,
                AssignedEmployeeCount = v.VehicleEmployees.Count,
                AssignedEmployeeRatio = v.VehicleEmployees.Count.ToString() + "/" + v.Capacity.ToString()
            });
        }

        public static IQueryable<VehicleEmployeeResult> ProjectToVehicleEmployeeResults(this IQueryable<VehicleEmployee> query)
        {
            return query.Select(ve => new VehicleEmployeeResult
            {
                VehicleId = ve.VehicleId,
                EmployeeId = ve.EmployeeId,
                VehicleDescription = ve.Vehicle != null ? ve.Vehicle.Description : string.Empty,
                VehicleCapacity = ve.Vehicle != null ? ve.Vehicle.Capacity : 0,
                EmployeeFirstName = ve.Employee != null ? ve.Employee.FirstName : string.Empty,
                EmployeeLastName = ve.Employee != null ? ve.Employee.LastName : string.Empty
            });
        }
    }
}
