using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            ViewBag.Message = result.Message;

            return RedirectToAction("GetAuctionSessionForUser", "AuctionSession", new { message = "Auction Session created successfully!" });
        }
    }
}
