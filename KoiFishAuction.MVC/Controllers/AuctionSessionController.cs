using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers
{
    public class AuctionSessionController : Controller
    {
        private readonly IAuctionSessionApiClient _auctionSessionApiClient;

        public AuctionSessionController(IAuctionSessionApiClient auctionSessionApiClient)
        {
            _auctionSessionApiClient = auctionSessionApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _auctionSessionApiClient.GetOngoingAuctionSessionAsync();

            ViewBag.Message = result.Message;

            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _auctionSessionApiClient.GetAuctionSessionByIdAsync(id);

            ViewBag.Message = result.Message;

            return View(result.Data);
        }
    }
}
