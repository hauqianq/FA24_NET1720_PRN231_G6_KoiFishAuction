using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Common.ViewModels.User;
using KoiFishAuction.Service.Services;

namespace KoiFishAuction.MVC.Services.Interfaces
{
    public interface IUserApiClient
    {
        Task<ServiceResult<bool>> RegisterUserAsync(RegisterUserRequestModel request);
        Task<ServiceResult<string>> LoginUserAsync(LoginUserRequestModel request);
        Task<ServiceResult<bool>> UpdateUserAsync(int id, UpdateUserRequestModel request);
        Task<ServiceResult<UserViewModel>> GetUserByIdAsync(int userid);
    }
}
