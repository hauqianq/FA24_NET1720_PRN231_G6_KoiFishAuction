using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel request)
        {
            var result = await _userService.RegisterUserAsync(request);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel request)
        {
            var result = await _userService.LoginUserAsync(request);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
