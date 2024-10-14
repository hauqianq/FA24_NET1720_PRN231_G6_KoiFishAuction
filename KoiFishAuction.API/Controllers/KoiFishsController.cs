using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiFishController : ControllerBase
    {
        private readonly IKoiFishService _koiFishService;

        public KoiFishController(IKoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetAllKoiFish(int userid)
        {
            var result = await _koiFishService.GetAllKoiFishesAsync(userid);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetKoiFishById(int id)
        {
            var result = await _koiFishService.GetKoiFishByIdAsync(id);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKoiFish([FromBody] CreateKoiFishRequestModel request, int sellerId)
        {
            var result = await _koiFishService.CreateKoiFishAsync(request, sellerId);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKoiFish(int id, [FromBody] UpdateKoiFishRequestModel request)
        {
            var result = await _koiFishService.UpdateKoiFishAsync(id, request);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKoiFish(int id)
        {
            var result = await _koiFishService.DeleteKoiFishAsync(id);
            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
