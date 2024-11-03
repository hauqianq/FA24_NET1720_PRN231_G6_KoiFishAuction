using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KoiFishController : ControllerBase
    {
        private readonly IKoiFishService _koiFishService;

        public KoiFishController(IKoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllKoiFish(int userId)
        {
            var result = await _koiFishService.GetAllKoiFishesAsync(userId);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetKoiFishById(int id)
        {
            var result = await _koiFishService.GetKoiFishByIdAsync(id);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKoiFish([FromForm] CreateKoiFishRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _koiFishService.CreateKoiFishAsync(request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKoiFish(int id, [FromForm] UpdateKoiFishRequestModel request, [FromForm] List<IFormFile> newImages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _koiFishService.UpdateKoiFishAsync(id, request, newImages);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKoiFish(int id)
        {
            var result = await _koiFishService.DeleteKoiFishAsync(id);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id}/price")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] decimal price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _koiFishService.UpdateKoiFishPriceAsync(id, price);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode)
            {
                BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
