using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IAuctionSessionService
    {
        Task<ServiceResult> CreateAuctionAsync(CreateAuctionSessionRequestModel request, int ownerid);
        Task<ServiceResult> GetOngoingAuctionsAsync(string search = null);
        Task<ServiceResult> GetAuctionsForUserAsync(int userid);
        Task<ServiceResult> GetAuctionByIdAsync(int id);
        Task<ServiceResult> SetAuctionWinnerAsync(int auctionId, int winnerId);
        Task<ServiceResult> ChangeAuctionStatusAsync(int id);
        Task<ServiceResult> UpdateAuctionAsync(UpdateAuctionSessionRequestModel request);
        Task<ServiceResult> CanUpdateAuctionAsync(int id);
    }
}
