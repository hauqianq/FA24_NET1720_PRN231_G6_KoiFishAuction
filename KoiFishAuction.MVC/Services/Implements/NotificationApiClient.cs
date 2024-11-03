using KoiFishAuction.Common.RequestModels.Notification;
using KoiFishAuction.Common.ViewModels.Notification;
using KoiFishAuction.Service.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KoiFishAuction.MVC.Services.Implements; 
public class NotificationApiClient {
    private readonly HttpClient _client;
   
    private const string NotificationEnpoint = $"/api/notifcations";

    public NotificationApiClient(HttpClient client) {
        _client = client;
    }

    public async Task<ServiceResult<int>> CreateNotificationAsync(CreateNotificationRequestModel request) {

        var formData = new MultipartFormDataContent {
            { new StringContent(request.UserId.ToString()), "UserId" },
            { new StringContent(request.ItemId.ToString()), "ItemId" },
            { new StringContent(request.Message), "Message" },
            { new StringContent(request.Type), "Type" },
            { new StringContent(request.BidId.ToString()), "BidId" },
            { new StringContent(request.Remarks), "Remarks" }
        };

        var response = await _client.PostAsync(NotificationEnpoint, formData);
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ServiceResult<int>>(result)!;
    }

    public async Task<ServiceResult<bool>> DeleteNotificationAsync(int id) {

        var response = await _client.DeleteAsync($"{NotificationEnpoint}/{id}");
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ServiceResult<bool>>(result)!;
    }

    public async Task<ServiceResult<List<NotificationViewModel>>> GetAllNotificationsAsync() {

        var response = await _client.GetAsync(NotificationEnpoint);
        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ServiceResult<List<NotificationViewModel>>>(result)!;
    }

    public async Task<ServiceResult<NotificationViewModel>> GetNotificationByIdAsync(int id) {

        var response = await _client.GetAsync($"{NotificationEnpoint}/{id}");
        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ServiceResult<NotificationViewModel>>(result)!;
    }

    public async Task<ServiceResult<int>> UpdateNotificationAsync(int id, UpdateNotificationRequestModel request) {

        var formData = new MultipartFormDataContent() {
            { new StringContent(request.Id.ToString()), "Id" },
            { new StringContent(request.Message), "Message" },
            { new StringContent(request.Type), "Type" },
            { new StringContent(request.IsRead.ToString()), "IsRead" },
            { new StringContent(request.Remarks), "Remarks" }
        };

        var response = await _client.PutAsync($"{NotificationEnpoint}/{id}", formData);
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ServiceResult<int>>(result)!;
    }
}
