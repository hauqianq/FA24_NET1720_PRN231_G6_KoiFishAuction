using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidsController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBidById(int id)
        {
            var result = await _bidService.GetBidByIdAsync(id);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{auctionSessionId}")]
        public async Task<IActionResult> GetAllBidForAuctionSessionAsync(int auctionSessionId)
        {
            var result = await _bidService.GetAllBidForAuctionSessionAsync(auctionSessionId);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("{userid}")]
        public async Task<IActionResult> CreateBid([FromBody] CreateBidRequestModel request, int userid)
        {
            var result = await _bidService.PlaceBidAsync(request, userid);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
