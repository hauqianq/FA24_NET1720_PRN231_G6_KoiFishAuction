using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers
{
    public class BidController : Controller
    {
        private readonly IBidApiClient _bidApiClient;

        public BidController(IBidApiClient bidApiClient)
        {
            _bidApiClient = bidApiClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBid(CreateBidRequestModel request)
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _bidApiClient.PlaceBidAsync(request);

            ViewBag.Message = result.Message;

            return RedirectToAction("Details", "AuctionSession", new { id = request.AuctionSessionId });
        }
    }
}
