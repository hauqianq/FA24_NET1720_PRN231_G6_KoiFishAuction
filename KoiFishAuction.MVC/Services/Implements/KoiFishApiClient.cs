using Azure.Core;
using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.MVC.Services.Interfaces;
using KoiFishAuction.Service.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KoiFishAuction.MVC.Services.Implements
{
    public class KoiFishApiClient : IKoiFishApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KoiFishApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<int>> CreateKoiFishAsync(CreateKoiFishRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(request.Name), "Name");
            formData.Add(new StringContent(request.Description), "Description");
            formData.Add(new StringContent(request.StartingPrice.ToString()), "StartingPrice");
            formData.Add(new StringContent(request.CurrentPrice.ToString()), "CurrentPrice");
            formData.Add(new StringContent(request.Age.ToString()), "Age");
            formData.Add(new StringContent(request.Origin), "Origin");
            formData.Add(new StringContent(request.Weight.ToString()), "Weight");
            formData.Add(new StringContent(request.Length.ToString()), "Length");
            formData.Add(new StringContent(request.ColorPattern), "ColorPattern");

            if (request.Images != null && request.Images.Count > 0)
            {
                foreach (var file in request.Images)
                {
                    if (file.Length > 0)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        formData.Add(streamContent, "Images", file.FileName);
                    }
                }
            }

            var response = await client.PostAsync($"/api/KoiFish", formData);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }

        public async Task<ServiceResult<bool>> DeleteKoiFishAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync($"/api/KoiFish/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<bool>>(result);
        }

        public async Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync(int userId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/KoiFish/user/{userId}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ServiceResult<List<KoiFishViewModel>>>(result);
        }

        public async Task<ServiceResult<KoiFishDetailViewModel>> GetKoiFishByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/KoiFish/{id}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ServiceResult<KoiFishDetailViewModel>>(result);
        }

        public async Task<ServiceResult<int>> UpdateKoiFishAsync(UpdateKoiFishRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/KoiFish", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }
    }
}
