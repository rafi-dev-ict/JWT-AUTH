using JWT_AUTH.Model;
using JWT_AUTH.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService jwtService;

        public AccountController(JwtService jwtService)
        {
            this.jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>>Login(LoginRequest loginRequest)
        {
            var result = await jwtService.Authenticate(loginRequest);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
    }
}
