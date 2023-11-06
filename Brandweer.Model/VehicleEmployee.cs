using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandweer.Model
{
    public class VehicleEmployee
    {
        public int Id { get; set; }
        public required int VehicleId { get; set; }
        public required int EmployeeId { get; set; }

        public Vehicle? Vehicle { get; set; }
        public Employee? Employee { get; set; }
    }
}
