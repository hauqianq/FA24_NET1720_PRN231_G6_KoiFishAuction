using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.Data.Models;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KoiFishAuction.MVC.Controllers
{
    public class KoiFishController : Controller
    {
        private readonly IKoiFishApiClient _koiFishApiClient;

        public KoiFishController(IKoiFishApiClient koiFishApiClient)
        {
            _koiFishApiClient = koiFishApiClient;
        }

        public string Message { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var result = await _koiFishApiClient.GetAllKoiFishesAsync();

            ViewBag.Message = message;

            return View(result.Data);
        }

        [HttpGet]
        public async Task<JsonResult> Search(string searchQuery = null, string sortOrder = null)
        {
            var result = await _koiFishApiClient.GetAllKoiFishesAsync();

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                var koiFishes = result.Data as List<KoiFishViewModel> ?? new List<KoiFishViewModel>();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    koiFishes = koiFishes.Where(k => k.Name.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase)).ToList();
                }

                switch (sortOrder)
                {
                    case "Name":
                        koiFishes = koiFishes.OrderBy(k => k.Name).ToList();
                        break;
                    case "Description":
                        koiFishes = koiFishes.OrderBy(k => k.Description).ToList();
                        break;
                    case "StartingPrice":
                        koiFishes = koiFishes.OrderBy(k => k.StartingPrice).ToList();
                        break;
                    case "CurrentPrice":
                        koiFishes = koiFishes.OrderBy(k => k.CurrentPrice).ToList();
                        break;
                    case "Age":
                        koiFishes = koiFishes.OrderBy(k => k.Age).ToList();
                        break;
                    case "Origin":
                        koiFishes = koiFishes.OrderBy(k => k.Origin).ToList();
                        break;
                    case "Weight":
                        koiFishes = koiFishes.OrderBy(k => k.Weight).ToList();
                        break;
                    case "Length":
                        koiFishes = koiFishes.OrderBy(k => k.Length).ToList();
                        break;
                    case "ColorPattern":
                        koiFishes = koiFishes.OrderBy(k => k.ColorPattern).ToList();
                        break;
                    case "SellerUsername":
                        koiFishes = koiFishes.OrderBy(k => k.SellerUserName).ToList();
                        break;
                    default:
                        koiFishes = koiFishes.OrderBy(k => k.Name).ToList();
                        break;
                }

                return new JsonResult(koiFishes);
            }

            return new JsonResult(new List<KoiFishViewModel>());
        }
    }
}
