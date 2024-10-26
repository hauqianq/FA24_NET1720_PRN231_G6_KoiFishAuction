using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.MVC.Services.Interfaces;
using KoiFishAuction.Service.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
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

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/KoiFish", httpContent);
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

        public async Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/KoiFish");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ServiceResult<List<KoiFishViewModel>>>(result);
        }

        public async Task<ServiceResult<KoiFishViewModel>> GetKoiFishByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var response = await client.GetAsync($"/api/KoiFish/{id}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ServiceResult<KoiFishViewModel>>(result);
        }

        public async Task<ServiceResult<int>> UpdateKoiFishAsync(int id, UpdateKoiFishRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/KoiFish/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }

        public async Task<ServiceResult<int>> UpdateKoiFishPriceAsync(int id, decimal price)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var json = JsonConvert.SerializeObject(new { Price = price });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/koifish/{id}/price", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }

    }
}
