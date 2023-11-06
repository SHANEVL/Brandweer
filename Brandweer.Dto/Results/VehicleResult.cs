namespace Brandweer.Dto.Results
{
    public class VehicleResult
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public required int Capacity { get; set; }
        public required int AssignedEmployeeCount { get; set; }
        public required string AssignedEmployeeRatio { get; set; }
    }
}
