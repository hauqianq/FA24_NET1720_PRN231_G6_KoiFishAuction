using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
