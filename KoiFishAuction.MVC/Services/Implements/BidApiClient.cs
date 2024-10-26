using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.Bid;
using KoiFishAuction.MVC.Services.Interfaces;
using KoiFishAuction.Service.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KoiFishAuction.MVC.Services.Implements
{
    public class BidApiClient : IBidApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BidApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<List<BidViewModel>>> GetAllBidForAuctionSessionAsync(int auctionSessionId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/bids/{auctionSessionId}");

            return JsonConvert.DeserializeObject<ServiceResult<List<BidViewModel>>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ServiceResult<bool>> PlaceBidAsync(CreateBidRequestModel request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/bids", httpContent);

            return JsonConvert.DeserializeObject<ServiceResult<bool>>(await response.Content.ReadAsStringAsync());
        }
    }
}
