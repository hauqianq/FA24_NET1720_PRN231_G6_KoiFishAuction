using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IAuctionSessionService
    {
        Task<ServiceResult<int>> CreateAuctionSessionAsync(CreateAuctionSessionRequestModel request);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionSessionAsync(string search = null);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionSessionForUserAsync();
        Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionSessionByIdAsync(int id);
        Task<ServiceResult<int>> UpdateAuctionSessionAsync(UpdateAuctionSessionRequestModel request);
        Task<ServiceResult<bool>> DeleteAuctionSessionAsync(int id);
    }
}
