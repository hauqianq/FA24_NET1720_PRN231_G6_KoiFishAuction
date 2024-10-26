using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.Bid;
using KoiFishAuction.Service.Services;

namespace KoiFishAuction.MVC.Services.Interfaces
{
    public interface IBidApiClient
    {
        Task<ServiceResult<bool>> PlaceBidAsync(CreateBidRequestModel request);
        Task<ServiceResult<List<BidViewModel>>> GetAllBidForAuctionSessionAsync(int auctionSessionId);
    }
}
