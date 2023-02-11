using System;
using System.Threading.Tasks;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.BackEnd.Services;
using EtqanArchive.Localization.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EtqanArchive.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;

        //private readonly ILogger _logger;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        // POST: api/Account/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                return Ok(result);
            }

            return BadRequest(Validation.InvalidSomeProperties);
        }


        // POST: api/Account/Logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _userService.SignOutAsync();
            return Ok(result);
        }



    }
}
