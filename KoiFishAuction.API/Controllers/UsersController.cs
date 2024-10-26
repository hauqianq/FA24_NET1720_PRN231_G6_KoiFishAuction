﻿using KoiFishAuction.Common.RequestModels.User;
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
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel request)
        {
            var result = await _userService.LoginUserAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LoginUserRequestModel request)
        {
            var result = await _userService.LoginUserAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
