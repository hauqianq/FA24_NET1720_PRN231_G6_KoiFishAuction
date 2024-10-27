using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using Microsoft.AspNetCore.Http;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IKoiFishService
    {
        Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync();
        Task<ServiceResult<KoiFishDetailViewModel>> GetKoiFishByIdAsync(int id);
        Task<ServiceResult<int>> CreateKoiFishAsync(CreateKoiFishRequestModel request);
        Task<ServiceResult<bool>> DeleteKoiFishAsync(int id);
        Task<bool> CanUpdateKoiFishAsync(int id);
        Task<ServiceResult<int>> UpdateKoiFishAsync(int id, UpdateKoiFishRequestModel request, List<IFormFile> newImages);
        Task<ServiceResult<int>> UpdateKoiFishPriceAsync(int id, decimal price);
    }
}
