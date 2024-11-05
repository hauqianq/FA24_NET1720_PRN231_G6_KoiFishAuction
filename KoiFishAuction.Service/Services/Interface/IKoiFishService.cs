using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IKoiFishService
    {
        Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync(int userId);
        Task<ServiceResult<KoiFishDetailViewModel>> GetKoiFishByIdAsync(int id);
        Task<ServiceResult<int>> CreateKoiFishAsync(CreateKoiFishRequestModel request);
        Task<ServiceResult<bool>> DeleteKoiFishAsync(int id);
        Task<ServiceResult<int>> UpdateKoiFishAsync(UpdateKoiFishRequestModel request);
    }
}
