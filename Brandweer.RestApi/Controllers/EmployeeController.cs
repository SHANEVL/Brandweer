using Brandweer.Dto.Requests;
using Brandweer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brandweer.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindAsync()
        {
            var employees = await _employeeService.FindAsync();
            return Ok(employees);
        }

        [HttpGet("{id:int}", Name = "GetEmployeeRoute")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var employeeResult = await _employeeService.GetAsync(id);
            if (employeeResult is null)
            {
                return NotFound();
            }

            return Ok(employeeResult);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _employeeService.CreateAsync(model);
            if (!serviceResult.IsSuccess || serviceResult.Result is null)
            {
                return BadRequest(serviceResult);
            }

            return CreatedAtRoute("GetEmployeeRoute", new { id = serviceResult.Result.Id }, serviceResult.Result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] EmployeeRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _employeeService.UpdateAsync(id, model);
            if (!serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult.Result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var serviceResult = await _employeeService.DeleteAsync(id);
            if (!serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult);
            }

            return NoContent(); 
        }
    }
}
