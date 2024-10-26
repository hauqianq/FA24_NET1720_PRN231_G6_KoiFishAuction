using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IAuctionSessionService
    {
        Task<ServiceResult<int>> CreateAuctionAsync(CreateAuctionSessionRequestModel request);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionsAsync(string search = null);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionsForUserAsync();
        Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionByIdAsync(int id);
        Task<ServiceResult<int>> SetAuctionWinnerAsync(int auctionId);
        Task<ServiceResult<bool>> ChangeAuctionStatusAsync(int id);
        Task<ServiceResult<int>> UpdateAuctionAsync(UpdateAuctionSessionRequestModel request);
        Task<ServiceResult<bool>> CanUpdateAuctionAsync(int id);
    }
}
