using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

            return View("~/Views/User/KoiFish/Index.cshtml", result.Data);
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
                    case "CurrentPrice":
                        koiFishes = koiFishes.OrderBy(k => k.CurrentPrice).ToList();
                        break;
                    case "Origin":
                        koiFishes = koiFishes.OrderBy(k => k.Origin).ToList();
                        break;
                    case "ColorPattern":
                        koiFishes = koiFishes.OrderBy(k => k.ColorPattern).ToList();
                        break;
                    default:
                        koiFishes = koiFishes.OrderBy(k => k.Name).ToList();
                        break;
                }

                return new JsonResult(koiFishes);
            }

            return new JsonResult(new List<KoiFishViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _koiFishApiClient.GetKoiFishByIdAsync(id);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return View(result.Data);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKoiFishRequestModel request)
        {
            var result = await _koiFishApiClient.CreateKoiFishAsync(request);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return RedirectToAction("~/Views/User/KoiFish/Index.cshtml", new { message = "Koi fish created successfully!" });
            }

            ViewBag.Message = result.Message;   

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var koiFish = (await _koiFishApiClient.GetKoiFishByIdAsync(id)).Data;
            if (koiFish == null)
            {
                return NotFound();
            }

            var model = new KoiFishDetailViewModel
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Description = koiFish.Description,
                StartingPrice = koiFish.StartingPrice,
                CurrentPrice = koiFish.CurrentPrice,
                Age = koiFish.Age,
                Origin = koiFish.Origin,
                Weight = koiFish.Weight,
                Length = koiFish.Length,
                ColorPattern = koiFish.ColorPattern,
                Images = koiFish.Images 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KoiFishDetailViewModel model, List<IFormFile> newImages)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new UpdateKoiFishRequestModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                StartingPrice = model.StartingPrice,
                CurrentPrice = model.CurrentPrice,
                Age = model.Age,
                Origin = model.Origin,
                Weight = model.Weight,
                Length = model.Length,
                ColorPattern = model.ColorPattern,
                ImageUrls = model.Images 
            };

            var result = await _koiFishApiClient.UpdateKoiFishAsync(model.Id, request, newImages);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction("~/Views/User/KoiFish/Details.cshtml", new { id = model.Id });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _koiFishApiClient.DeleteKoiFishAsync(id);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                ViewBag.Message = result.Message;
                return RedirectToAction("~/Views/User/KoiFish/Index.cshtml");
            }
            else
            {
                ViewBag.Message = result.Message;
                return RedirectToAction("~/Views/User/KoiFish/Index.cshtml");
            }
        }

    }
}
