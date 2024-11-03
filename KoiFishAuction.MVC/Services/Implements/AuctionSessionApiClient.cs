using Azure.Core;
using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.MVC.Services.Interfaces;
using KoiFishAuction.Service.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KoiFishAuction.MVC.Services.Implements
{
    public class AuctionSessionApiClient : IAuctionSessionApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuctionSessionApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<int>> CreateAuctionSessionAsync(CreateAuctionSessionRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/auctionsessions", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }

        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionSessionAsync(string search = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var response = await client.GetAsync($"/api/auctionsessions/ongoing?search={search}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<List<AuctionSessionViewModel>>>(result);
        }

        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionSessionForUserAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/auctionsessions/user");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<List<AuctionSessionViewModel>>>(result);
        }

        public async Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionSessionByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var response = await client.GetAsync($"/api/auctionsessions/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<AuctionSessionDetailViewModel>>(result);
        }

        public async Task<ServiceResult<int>> UpdateAuctionSessionAsync(UpdateAuctionSessionRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/auctionsessions", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(result);
        }

        public async Task<ServiceResult<bool>> DeleteAuctionSessionAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync($"/api/auctionsessions/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<bool>>(result);
        }
    }
}
