using Brandweer.Core;
using Brandweer.Dto.Requests;
using Brandweer.Dto.Results;
using Brandweer.Model;
using Brandweer.Services.Extensions;
using Brandweer.Services.Model;
using Brandweer.Services.Model.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Brandweer.Services
{
    public class VehicleService
    {
        private readonly BrandweerDbContext _dbContext;

        public VehicleService(BrandweerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Lijst en detail voor een dashboard: 10p

        // Method to get the list of vehicles with assigned employee count and ratio
        public async Task<ServiceResult<List<VehicleResult>>> GetVehiclesWithAssignedCountAndRatioAsync()
        {
            var vehicles = await _dbContext.Vehicles
                .Include(v => v.VehicleEmployees)
                .ProjectToVehicleResults() 
                .ToListAsync();

            return new ServiceResult<List<VehicleResult>>(vehicles);
        }

        // Method to get a single vehicle with the assigned employee count and ratio
        public async Task<ServiceResult<VehicleResult>> GetVehicleWithAssignedCountAndRatioAsync(int vehicleId)
        {
            var vehicleResult = await _dbContext.Vehicles
                .Where(v => v.Id == vehicleId)
                .Include(v => v.VehicleEmployees)
                .ProjectToVehicleResults() 
                .SingleOrDefaultAsync();

            if (vehicleResult != null)
            {
                return new ServiceResult<VehicleResult>(vehicleResult);
            }
            else
            {
                var notFoundResult = new ServiceResult<VehicleResult>
                {
                    Messages = {
                        new ServiceMessage {
                            Code = "VehicleNotFound",
                            Title = "Vehicle not found.",
                            Type = ServiceMessageType.Error
                        }
                    }
                };
                return notFoundResult;
            }
        }

        // CRUD Vehicles
        //Find
        public async Task<List<VehicleResult>> FindAsync()
        {
            return await _dbContext.Vehicles
                .ProjectToVehicleResults()
                .ToListAsync();
        }

        //Get by id
        public async Task<VehicleResult?> GetAsync(int id)
        {
            return await _dbContext.Vehicles
                .ProjectToVehicleResults()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<ServiceResult<VehicleResult?>> CreateAsync(VehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                Description = request.Description,
                Capacity = request.Capacity 
            };

            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            var vehicleResult = await GetAsync(vehicle.Id);

            return new ServiceResult<VehicleResult?>(vehicleResult);
        }

        //Update
        public async Task<ServiceResult<VehicleResult?>> UpdateAsync(int id, VehicleRequest request)
        {
            var dbVehicle = await _dbContext.Vehicles.FindAsync(id);
            if (dbVehicle == null)
            {
                return new ServiceResult<VehicleResult?>().NotFound("Vehicle not found.");
            }

            dbVehicle.Description = request.Description;
            dbVehicle.Capacity = request.Capacity; 

            await _dbContext.SaveChangesAsync();

            var vehicleResult = await GetAsync(id);
            return new ServiceResult<VehicleResult?>(vehicleResult);
        }

        //Delete
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return new ServiceResult().NotFound("Vehicle not found.");
            }

            _dbContext.Vehicles.Remove(vehicle);

            await _dbContext.SaveChangesAsync();

            return new ServiceResult();
        }


    }
}

