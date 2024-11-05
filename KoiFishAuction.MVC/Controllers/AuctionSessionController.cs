using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KoiFishAuction.MVC.Controllers
{
    public class AuctionSessionController : Controller
    {
        private readonly IAuctionSessionApiClient _auctionSessionApiClient;
        private readonly IKoiFishApiClient _koiFishApiClient;
        public AuctionSessionController(IAuctionSessionApiClient auctionSessionApiClient, IKoiFishApiClient koiFishApiClient)
        {
            _auctionSessionApiClient = auctionSessionApiClient;
            _koiFishApiClient = koiFishApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _auctionSessionApiClient.GetOngoingAuctionSessionAsync();

            if (result.Message != null)
            {
                ViewBag.Message = result.Message;
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<JsonResult> SearchAuctionSessions(string searchName = null, string searchKoiFishName = null, string searchPrice = null, string sortOrder = null)
        {
            var result = await _auctionSessionApiClient.GetOngoingAuctionSessionAsync();
            var sessions = result.Data;

            if (!string.IsNullOrEmpty(searchName))
            {
                sessions = sessions.Where(s => s.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(searchKoiFishName))
            {
                sessions = sessions.Where(s => s.KoiFishName.Contains(searchKoiFishName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(searchPrice) && decimal.TryParse(searchPrice, out var price))
            {
                sessions = sessions.Where(s => s.Price == price).ToList();
            }

            sessions = sortOrder switch
            {
                "Name" => sessions.OrderBy(s => s.Name).ToList(),
                "KoiFishName" => sessions.OrderBy(s => s.KoiFishName).ToList(),
                "Price" => sessions.OrderBy(s => s.Price).ToList(),
                "StartTime" => sessions.OrderBy(s => s.StartTime).ToList(),
                "EndTime" => sessions.OrderBy(s => s.EndTime).ToList(),
                "Status" => sessions.OrderBy(s => s.Status).ToList(),
                _ => sessions.OrderBy(s => s.Name).ToList(),
            };

            return Json(sessions);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _auctionSessionApiClient.GetAuctionSessionByIdAsync(id);

            ViewBag.Message = result.Message;

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAuctionSessionForUser(string message)
        {
            var result = await _auctionSessionApiClient.GetAuctionSessionForUserAsync();

            if (result.Message != null)
            {
                ViewBag.Message = result.Message;
            }

            ViewBag.Message = message;

            return View("~/Views/User/AuctionSession/Index.cshtml", result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var id = HttpContext.Session.GetInt32("id").Value;
            var koifishes = await _koiFishApiClient.GetAllKoiFishesAsync(id);
            var result = koifishes.Data;
            ViewBag.KoiFishList = new SelectList(result, "Id", "Name");
            return View("~/Views/User/AuctionSession/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuctionSessionRequestModel request)
        {
            var result = await _auctionSessionApiClient.CreateAuctionSessionAsync(request);

            return RedirectToAction("GetAuctionSessionForUser", "AuctionSession", new { message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _auctionSessionApiClient.DeleteAuctionSessionAsync(id);

            return RedirectToAction("GetAuctionSessionForUser", "AuctionSession", new { message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var auctionSession = (await _auctionSessionApiClient.GetAuctionSessionByIdAsync(id)).Data;
            if (auctionSession == null)
            {
                return NotFound();
            }

            var model = new UpdateAuctionSessionRequestModel
            {
                Id = auctionSession.Id,
                Name = auctionSession.Name,
                Note = auctionSession.Note,
                StartTime = auctionSession.StartTime,
                EndTime = auctionSession.EndTime,
                MinIncrement = auctionSession.MinIncrement
            };

            return View("~/Views/User/AuctionSession/Edit.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAuctionSessionRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/User/AuctionSession/Edit.cshtml", request);
            }
            var result = await _auctionSessionApiClient.UpdateAuctionSessionAsync(request);

            return RedirectToAction("GetAuctionSessionForUser", "AuctionSession", new { message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> OwnerDetails(int id)
        {
            var result = await _auctionSessionApiClient.GetAuctionSessionByIdAsync(id);

            ViewBag.Message = result.Message;

            return View("~/Views/User/AuctionSession/OwnerDetails.cshtml", result.Data);
        }
    }
}
