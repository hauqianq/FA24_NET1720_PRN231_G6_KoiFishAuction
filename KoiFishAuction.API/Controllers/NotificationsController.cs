using KoiFishAuction.Common.RequestModels.Notification;
using KoiFishAuction.Service.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class NotificationsController : ControllerBase {
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotifications([FromQuery] GetNotificationsRequestModel request) {
        var result = await _service.GetAllNotifications(request);
        if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
            BadRequest(result.Message);
        }
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetKoiFishById(int id) {
        var result = await _service.GetNotificationById(id);
        if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
            BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromForm] CreateNotificationRequestModel request) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var result = await _service.AddNotification(request);
        if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
            BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNotification(int id, [FromForm] UpdateNotificationRequestModel request) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var result = await _service.UpdateNotification(id, request);

        if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id) {
        var result = await _service.DeleteNotification(id);
        if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
            BadRequest(result.Message);
        }
        return Ok(result);
    }
}
