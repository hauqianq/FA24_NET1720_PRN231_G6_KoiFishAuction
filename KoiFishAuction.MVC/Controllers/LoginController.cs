using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace KoiFishAuction.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.LoginUserAsync(request);

            if (result.Message != null)
            {
                ViewBag.Message = result.Message;
            }

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                HttpContext.Session.SetString(Common.Constant.Token, result.Data);
                DecodeAndStoreUserSession(result.Data);
                return RedirectToAction("Index", "AuctionSession");
            }

            return View(request);
        }


        private void DecodeAndStoreUserSession(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            HttpContext.Session.SetString("UserName", JsonConvert.SerializeObject(jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value));
        }

    }
}
