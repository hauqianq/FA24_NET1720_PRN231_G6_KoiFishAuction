using KoiFishAuction.Common.RequestModels.AuctionHistory;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface;

public interface IAuctionHistoryService
{
    Task<ServiceResult> GetAllAuctionHistory(AuctionHistoryParams auctionHistoryParams);
}