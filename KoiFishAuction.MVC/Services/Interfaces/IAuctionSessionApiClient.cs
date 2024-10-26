using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.Service.Services;

namespace KoiFishAuction.MVC.Services.Interfaces
{
    public interface IAuctionSessionApiClient
    {
        Task<ServiceResult<int>> CreateAuctionSessionAsync(CreateAuctionSessionRequestModel request);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionSessionAsync(string search = null);
        Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionSessionForUserAsync();
        Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionSessionByIdAsync(int id);
        Task<ServiceResult<int>> SetAuctionSessionWinnerAsync(int id);
        Task<ServiceResult<bool>> ChangeAuctionSessionStatusAsync(int id);
        Task<ServiceResult<int>> UpdateAuctionSessionAsync(UpdateAuctionSessionRequestModel request);
    }
}
