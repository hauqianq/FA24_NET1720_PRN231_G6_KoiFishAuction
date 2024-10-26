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
            var result = await _auctionSessionService.CreateAuctionAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("ongoing")]
        public async Task<IActionResult> GetOngoingAuctions([FromQuery] string search = null)
        {
            var result = await _auctionSessionService.GetOngoingAuctionsAsync(search);
            return Ok(result);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetAuctionsForUser()
        {
            var result = await _auctionSessionService.GetAuctionsForUserAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var result = await _auctionSessionService.GetAuctionByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}/winner")]
        [Authorize]
        public async Task<IActionResult> SetAuctionWinner(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auctionSessionService.SetAuctionWinnerAsync(id);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> ChangeAuctionStatus(int id)
        {
            var result = await _auctionSessionService.ChangeAuctionStatusAsync(id);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAuction([FromBody] UpdateAuctionSessionRequestModel request)
        {
            var result = await _auctionSessionService.UpdateAuctionAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
