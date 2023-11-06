using Brandweer.Dto.Requests;
using Brandweer.RestApi.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Brandweer.RestApi.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("api/identity/sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var result = await _identityService.SignInAsync(request);
            return Ok(result);
        }

        [HttpPost("api/identity/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
