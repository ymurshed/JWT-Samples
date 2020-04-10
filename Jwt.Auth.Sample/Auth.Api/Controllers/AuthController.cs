using System.Collections.Generic;
using Auth.Contracts;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult GetUserToken(string name = Constants.AdminUserName, string email = Constants.AdminUserEmail)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                return NotFound();

            var user = _userService.GetUser(name, email);
            var token = _jwtService.GetToken(user);
            return Ok(token);
        }

        [HttpGet("users")]
        [Authorize(Policy = Constants.AdminUserPolicy)]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetAllUsers();
        }
    }
}
