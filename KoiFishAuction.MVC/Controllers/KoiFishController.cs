using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers
{
    public class KoiFishController : Controller
    {
        private readonly IKoiFishApiClient _koiFishApiClient;

        private readonly int PageIndex = 1;
        private readonly int PageSize = 2;

        public KoiFishController(IKoiFishApiClient koiFishApiClient)
        {
            _koiFishApiClient = koiFishApiClient;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string message, int page = 1, int pageSize = 2)
        {
            var id = HttpContext.Session.GetInt32("id").Value;

            var result = await _koiFishApiClient.GetAllKoiFishesAsync(id);

            var koiFishes = result.Data ?? new List<KoiFishViewModel>();

            int counts = koiFishes.Count;
            int totalPages = (int)Math.Ceiling((double)counts / pageSize);

            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var paginatedKoiFishes = koiFishes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Message = message;

            return View("~/Views/User/KoiFish/Index.cshtml", paginatedKoiFishes);
        }

        [HttpGet]
        public async Task<JsonResult> SearchKoiFishes(string searchName = null, string searchOrigin = null, string searchColorPattern = null, string sortOrder = null)
        {
            var id = HttpContext.Session.GetInt32("id").Value;

            var result = await _koiFishApiClient.GetAllKoiFishesAsync(id);
            var koiFishes = result.Data;

            // Filter by name
            if (!string.IsNullOrEmpty(searchName))
            {
                koiFishes = koiFishes.Where(k => k.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by origin
            if (!string.IsNullOrEmpty(searchOrigin))
            {
                koiFishes = koiFishes.Where(k => k.Origin.Contains(searchOrigin, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by color pattern
            if (!string.IsNullOrEmpty(searchColorPattern))
            {
                koiFishes = koiFishes.Where(k => k.ColorPattern.Contains(searchColorPattern, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sort the results based on the sortOrder
            koiFishes = sortOrder switch
            {
                "Name" => koiFishes.OrderBy(k => k.Name).ToList(),
                "CurrentPrice" => koiFishes.OrderBy(k => k.CurrentPrice).ToList(),
                "Origin" => koiFishes.OrderBy(k => k.Origin).ToList(),
                "ColorPattern" => koiFishes.OrderBy(k => k.ColorPattern).ToList(),
                _ => koiFishes.OrderBy(k => k.Name).ToList(),
            };

            return Json(koiFishes);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _koiFishApiClient.GetKoiFishByIdAsync(id);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return View("~/Views/User/KoiFish/Details.cshtml", result.Data);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/User/KoiFish/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKoiFishRequestModel request)
        {
            var result = await _koiFishApiClient.CreateKoiFishAsync(request);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return RedirectToAction("Index", "KoiFish", new { message = result.Message });
            }

            return View("~/Views/User/KoiFish/Create.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var koiFish = (await _koiFishApiClient.GetKoiFishByIdAsync(id)).Data;
            if (koiFish == null)
            {
                return NotFound();
            }

            var model = new UpdateKoiFishRequestModel
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
            };

            return View("~/Views/User/KoiFish/Edit.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKoiFishRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/User/KoiFish/Edit.cshtml", request);
            }

            var result = await _koiFishApiClient.UpdateKoiFishAsync(request);


            return RedirectToAction("Index", "KoiFish", new { message = result.Message });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _koiFishApiClient.DeleteKoiFishAsync(id);

            return RedirectToAction("Index", "KoiFish", new { message = result.Message });
        }

    }
}
