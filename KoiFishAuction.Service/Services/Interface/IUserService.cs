using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Common.ViewModels.User;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IUserService
    {
        Task<ServiceResult<bool>> RegisterUserAsync(RegisterUserRequestModel request);
        Task<ServiceResult<string>> LoginUserAsync(LoginUserRequestModel request);
        Task<ServiceResult<bool>> UpdateUserAsync(UpdateUserRequestModel request);
        Task<ServiceResult<UserViewModel>> GetUserByIdAsync(int userid);
    }
}
