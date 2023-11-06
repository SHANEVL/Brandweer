using Microsoft.AspNetCore.Mvc;
using Brandweer.Dto.Requests;
using Brandweer.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Brandweer.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeVehicleController : ControllerBase
    {
        private readonly VehicleEmployeeService _vehicleEmployeeService;

        public EmployeeVehicleController(VehicleEmployeeService vehicleEmployeeService)
        {
            _vehicleEmployeeService = vehicleEmployeeService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEmployeeToVehicle([FromBody] VehicleEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _vehicleEmployeeService.AssignEmployeeToVehicleAsync(request);
            if (!serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.Messages);
            }

            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveEmployeeFromVehicle([FromBody] VehicleEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _vehicleEmployeeService.RemoveEmployeeFromVehicleAsync(request.VehicleId, request.EmployeeId);
            if (!serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.Messages);
            }

            return Ok();
        }
    }
}
