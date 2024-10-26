using KoiFishAuction.Common.RequestModels.AuctionHistory;
using KoiFishAuction.Common.ViewModels.AuctionHistory;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface;

public interface IAuctionHistoryService
{
    Task<ServiceResult<PagedResponse<AuctionHistoryViewModel>>> GetAllAuctionHistory(AuctionHistoryParams auctionHistoryParams);
}