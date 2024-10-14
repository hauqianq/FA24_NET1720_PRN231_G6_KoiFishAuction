using JewelryAuction.Business.Business.Implementation;
using JewelryAuction.Business.RequestModels.Auction;
using JewelryAuction.Business.RequestModels.Bid;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IAuctionSessionService
    {
        Task<JewelryAuctionResult> CreateAuctionAsync(CreateAuctionRequestModel request, int ownerid);
        Task<JewelryAuctionResult> GetOngoingAuctionsAsync(string search = null);
        Task<JewelryAuctionResult> GetAuctionsForUserAsync(int userid);
        Task<JewelryAuctionResult> GetAuctionByIdAsync(int id);
        Task<JewelryAuctionResult> PlaceBidAsync(CreateBidRequestModel request);
        Task<JewelryAuctionResult> SetAuctionWinnerAsync(int auctionId, int winnerId);
        Task<JewelryAuctionResult> ChangeAuctionStatusAsync(int id);
        Task<JewelryAuctionResult> UpdateAuctionAsync(UpdateAuctionRequestModel request);
        Task<JewelryAuctionResult> CanUpdateAuctionAsync(int id);
    }
}
