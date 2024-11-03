using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BidsController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidsController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet("{auctionSessionId}")]
        public async Task<IActionResult> GetAllBidForAuctionSessionAsync(int auctionSessionId)
        {
            var result = await _bidService.GetAllBidForAuctionSessionAsync(auctionSessionId);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBid([FromBody] CreateBidRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bidService.PlaceBidAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
