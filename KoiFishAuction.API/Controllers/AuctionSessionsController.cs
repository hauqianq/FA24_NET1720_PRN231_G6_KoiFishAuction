using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionSessionsController : ControllerBase
    {
        private readonly IAuctionSessionService _auctionSessionService;

        public AuctionSessionsController(IAuctionSessionService auctionSessionService)
        {
            _auctionSessionService = auctionSessionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionSessionRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auctionSessionService.CreateAuctionSessionAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("ongoing")]
        public async Task<IActionResult> GetOngoingAuctions([FromQuery] string search = null)
        {
            var result = await _auctionSessionService.GetOngoingAuctionSessionAsync(search);
            return Ok(result);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetAuctionsForUser()
        {
            var result = await _auctionSessionService.GetAuctionSessionForUserAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var result = await _auctionSessionService.GetAuctionSessionByIdAsync(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAuction([FromBody] UpdateAuctionSessionRequestModel request)
        {
            var result = await _auctionSessionService.UpdateAuctionSessionAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var result = await _auctionSessionService.DeleteAuctionSessionAsync(id);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
