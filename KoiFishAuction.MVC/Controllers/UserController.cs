using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Common.ViewModels.User;
using KoiFishAuction.Data.Models;
using KoiFishAuction.MVC.Services.Interfaces;
using KoiFishAuction.Service.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserApiClient userApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View("~/Views/User/Profile/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userApiClient.RegisterUserAsync(model);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return RedirectToAction("Index", "AuctionSession");
            }

            ViewBag.Message = result.Message;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var id = HttpContext.Session.GetInt32("id").Value;

            var result = await _userApiClient.GetUserByIdAsync(id);
            var data = result.Data;

            var user = new UpdateUserRequestModel
            {
                UserId = data.Id,
                Username = data.Username,
                Password = data.Password,
                Balance = data.Balance,
                Email = data.Email,
            };

            return View("~/Views/User/Profile/Update.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userApiClient.UpdateUserAsync(model.UserId, model);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return RedirectToAction("Index", "AuctionSession");
            }

            ViewBag.Message = result.Message;

            return View("~/Views/User/Profile/Update.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
    }
}
