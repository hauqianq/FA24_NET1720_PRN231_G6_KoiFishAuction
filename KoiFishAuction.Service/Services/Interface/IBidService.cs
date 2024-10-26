using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.Bid;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IBidService
    {
        Task<ServiceResult<bool>> PlaceBidAsync(CreateBidRequestModel request);
        Task<ServiceResult<List<BidViewModel>>> GetAllBidForAuctionSessionAsync(int auctionSessionId);
    }
}
