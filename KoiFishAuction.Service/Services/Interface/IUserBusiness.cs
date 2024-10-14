using JewelryAuction.Business.Business.Implementation;
using JewelryAuction.Business.RequestModels.User;

namespace JewelryAuction.Business.Business.Interface
{
    public interface IUserBusiness
    {
        Task<JewelryAuctionResult> RegisterAsync(RegisterRequestModel request);
        Task<JewelryAuctionResult> LoginAsync(LoginRequestModel request);
        Task<JewelryAuctionResult> UpdateUserAsync(UpdateUserRequestModel request);
        Task<JewelryAuctionResult> GetUserByIdAsync(int userid);
    }
}
