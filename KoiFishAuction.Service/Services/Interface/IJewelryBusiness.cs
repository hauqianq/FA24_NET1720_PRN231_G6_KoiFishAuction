using JewelryAuction.Business.Business.Implementation;
using JewelryAuction.Business.RequestModels.Jewelry;

namespace JewelryAuction.Business.Business.Interface
{
    public interface IJewelryBusiness
    {
        Task<JewelryAuctionResult> GetAllJewelriesAsync(int sellerid);
        Task<JewelryAuctionResult> GetJewelryByIdAsync(int id);
        Task<JewelryAuctionResult> CreateJewelryAsync(CreateJewelryRequestModel request, int sellerid);
        Task<JewelryAuctionResult> DeleteJewelryAsync(int id);
        Task<JewelryAuctionResult> CanUpdateJewelryAsync(int id);
        Task<JewelryAuctionResult> UpdateJewelryAsync(UpdateJewelryRequestModel request, int sellerid);
        Task<JewelryAuctionResult> UpdateJewelryPriceAsync(int jewelryid, decimal price);
    }
}
