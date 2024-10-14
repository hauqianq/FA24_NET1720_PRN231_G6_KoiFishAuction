using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IKoiFishService
    {
        Task<ServiceResult> GetAllKoiFishesAsync(int sellerid);
        Task<ServiceResult> GetKoiFishByIdAsync(int id);
        Task<ServiceResult> CreateKoiFishAsync(CreateKoiFishRequestModel request, int sellerid);
        Task<ServiceResult> DeleteKoiFishAsync(int id);
        Task<ServiceResult> CanUpdateKoiFishAsync(int id);
        Task<ServiceResult> UpdateKoiFishAsync(int id, UpdateKoiFishRequestModel request);
        Task<ServiceResult> UpdateKoiFishPriceAsync(int jewelryid, decimal price);
    }
}
