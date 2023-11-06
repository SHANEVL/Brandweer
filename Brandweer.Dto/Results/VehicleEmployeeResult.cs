namespace Brandweer.Dto.Results
{
    public class VehicleEmployeeResult
    {
        public int VehicleId { get; set; }
        public int EmployeeId { get; set; }

        // Vehicle information
        public required string VehicleDescription { get; set; }
        public required int VehicleCapacity { get; set; }

        // Employee information
        public required string EmployeeFirstName { get; set; }
        public required string EmployeeLastName { get; set; }
    }
}
