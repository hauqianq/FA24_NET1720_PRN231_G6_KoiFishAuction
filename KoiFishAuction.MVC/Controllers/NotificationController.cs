using KoiFishAuction.Common.RequestModels.Notification;
using KoiFishAuction.Common.ViewModels.Notification;
using KoiFishAuction.MVC.Services.Implements;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers {
    public class NotificationController : Controller {
        private readonly NotificationApiClient _notificationApiClient;

        public NotificationController(NotificationApiClient notificationApiClient) {
            _notificationApiClient = notificationApiClient;
        }

        public string Message { get; set; }

        // GET: NotificationController
        public async Task<ActionResult> Index(string message) {
            var result = await _notificationApiClient.GetAllNotificationsAsync();

            ViewBag.Message = message;

            return View(result.Data);
        }

        // GET: NotificationController/Details/5
        public async Task<ActionResult> Details(int id) {
            var result = await _notificationApiClient.GetNotificationByIdAsync(id);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode) {
                return View(result.Data);
            }

            return NotFound();
        }

        // GET: NotificationController/Create
        public ActionResult Create() {
            return View();
        }

        // POST: NotificationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateNotificationRequestModel request) {
            var result = await _notificationApiClient.CreateNotificationAsync(request);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode) {
                return RedirectToAction("Index", new { message = "Notification created successfully!" });
            }

            ViewBag.Message = result.Message;

            return View();
        }

        // GET: NotificationController/Edit/5
        public async Task<ActionResult> Edit(int id) {
            var noti = (await _notificationApiClient.GetNotificationByIdAsync(id)).Data;
            if (noti == null) {
                return NotFound();
            }

            return View(noti);
        }

        // POST: NotificationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(NotificationViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var request = new UpdateNotificationRequestModel {
                Id = model.Id,
                Message = model.Message,
                Type = model.Type,
                IsRead = model.IsRead,
                Remarks = model.Remarks!
            };

            var result = await _notificationApiClient.UpdateNotificationAsync(model.Id, request);
            if (result.Status == Common.Constant.StatusCode.FailedStatusCode) {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.Id });
        }

        // GET: NotificationController/Delete/5
        //public ActionResult Delete(int id) {
        //    return View();
        //}

        // POST: NotificationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id) {
            var result = await _notificationApiClient.DeleteNotificationAsync(id);

            if (result.Status == Common.Constant.StatusCode.SuccessStatusCode) {
                ViewBag.Message = result.Message;
                return RedirectToAction("Index");
            }
            else {
                ViewBag.Message = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
