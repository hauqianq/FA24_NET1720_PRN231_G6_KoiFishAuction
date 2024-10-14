using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Common.ViewModels.User;
using KoiFishAuction.Service.Services.Implementation;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterUserRequestModel request);
        Task<ServiceResult> LoginUserAsync(LoginUserRequestModel request);
        Task<ServiceResult> UpdateUserAsync(UpdateUserRequestModel request);
        Task<ServiceResult> GetUserByIdAsync(int userid);
    }
}
