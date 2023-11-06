using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandweer.Model
{
    public class Vehicle
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public required int Capacity { get; set; }

        // To get the count in the result DTO
        public ICollection<VehicleEmployee> VehicleEmployees { get; set; } = new List<VehicleEmployee>();
    }
}
