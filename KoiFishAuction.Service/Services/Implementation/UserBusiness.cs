using JewelryAuction.Business.Business.Interface;
using JewelryAuction.Business.RequestModels.User;
using JewelryAuction.Data;
using JewelryAuction.Data.Models;

namespace JewelryAuction.Business.Business.Implementation
{
    public class UserBusiness : IUserBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        public UserBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JewelryAuctionResult> GetUserByIdAsync(int userid)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userid);
                if (user == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode);
                }
                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, user);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> LoginAsync(LoginRequestModel request)
        {
            try
            {
                var result = await _unitOfWork.UserRepository.LoginAsync(request.Username, request.Password);
                if (result != null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Login successfully.", result);
                }
                else
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "Login failed.");
                }
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> RegisterAsync(RegisterRequestModel request)
        {
            try
            {
                var user = new User()
                {
                    Username = request.Username,
                    Password = request.Password,
                    Email = request.Email,
                    Balance = 100000
                };

                await _unitOfWork.UserRepository.CreateAsync(user);
                await _unitOfWork.UserRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Register successfully.", user);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> UpdateUserAsync(UpdateUserRequestModel request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "User not found.");
                }

                if (request.Password != user.Password)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "Incorrect password.");
                }

                if (request.Balance <= 0 || request.Balance <= user.Balance)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The new balance must be greater than current balance.");
                }

                user.Balance = request.Balance;
                user.Email = request.Email;
                user.Password = request.Password;
                user.Username = request.Username;

                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.UserRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Update user successfully.", user);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
