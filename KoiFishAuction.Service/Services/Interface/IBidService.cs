using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IBidService
    {
        Task<ServiceResult> PlaceBidAsync(CreateBidRequestModel request, int bidderid);
        Task<ServiceResult> GetBidByIdAsync(int id);
        Task<ServiceResult> GetAllBidForAuctionSessionAsync(int auctionSessionId);
    }
}
