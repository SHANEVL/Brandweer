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
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;

        public VehiclesController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindAsync()
        {
            var vehicles = await _vehicleService.FindAsync();
            return Ok(vehicles); 
        }

        [HttpGet("{id:int}", Name = "GetVehicleRoute")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var serviceResult = await _vehicleService.GetAsync(id); 
            if (serviceResult == null)
            {
                return NotFound();
            }
            return Ok(serviceResult);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateAsync([FromBody] VehicleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _vehicleService.CreateAsync(request); 
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.Messages);
            }

            return CreatedAtRoute("GetVehicleRoute", new { id = serviceResult.Result.Id }, serviceResult.Result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] VehicleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _vehicleService.UpdateAsync(id, request); 
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.Messages);
            }

            return Ok(serviceResult.Result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var serviceResult = await _vehicleService.DeleteAsync(id); 
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.Messages);
            }

            return NoContent();
        }

        [HttpGet("assigned")]
        public async Task<IActionResult> GetVehiclesWithAssignedCountAndRatio()
        {
            var serviceResult = await _vehicleService.GetVehiclesWithAssignedCountAndRatioAsync();
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return NotFound(serviceResult.Messages);
            }
            return Ok(serviceResult.Result);
        }

        [HttpGet("{id:int}/assigned")]
        public async Task<IActionResult> GetVehicleWithAssignedCountAndRatio([FromRoute] int id)
        {
            var serviceResult = await _vehicleService.GetVehicleWithAssignedCountAndRatioAsync(id);
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return NotFound(serviceResult.Messages);
            }
            return Ok(serviceResult.Result);
        }
    }
}
