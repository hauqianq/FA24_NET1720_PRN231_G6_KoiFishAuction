using KoiFishAuction.Common.RequestModels.User;
using KoiFishAuction.Common.ViewModels.User;
using KoiFishAuction.Data;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public UserService(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ServiceResult<UserViewModel>> GetUserByIdAsync(int userid)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userid);
                if (user == null)
                {
                    return new ServiceResult<UserViewModel>(Common.Constant.StatusCode.FailedStatusCode);
                }

                var data = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Balance = user.Balance,
                    Address = user.Address,
                    FullName = user.FullName,
                    JoinDate = user.JoinDate,
                    LastLogin = user.LastLogin,
                    PhoneNumber = user.PhoneNumber
                };

                return new ServiceResult<UserViewModel>(Common.Constant.StatusCode.SuccessStatusCode, data);
            }
            catch (Exception e)
            {
                return new ServiceResult<UserViewModel>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<string>> LoginUserAsync(LoginUserRequestModel request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.LoginAsync(request);

                if (user == null)
                {
                    return new ServiceResult<string>(Common.Constant.StatusCode.FailedStatusCode, "User does not exist.");
                }

                var userClaims = new[]
                {
                new Claim("id",user.Id.ToString()),
                new Claim("username",user.Username),
                new Claim("emailaddress", user.Email),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                    (
                        issuer: _configuration["JwtConfiguration:Issuer"],
                        audience: _configuration["JwtConfiguration:Issuer"],
                        claims: userClaims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: creds
                    );

                return new ServiceResult<string>(Common.Constant.StatusCode.SuccessStatusCode, "Login successfully.", new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception e)
            {
                return new ServiceResult<string>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> RegisterUserAsync(RegisterUserRequestModel request)
        {
            try
            {
                var result = await _unitOfWork.UserRepository.RegisterAsync(request);

                if (result == null)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "Register failed.");
                }

                return new ServiceResult<bool>(Common.Constant.StatusCode.SuccessStatusCode, "Register successfully.");
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(UpdateUserRequestModel request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "User not found.");
                }

                if (request.Password != user.Password)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "Incorrect password.");
                }

                if (request.Balance <= 0 || request.Balance <= user.Balance)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "The new balance must be greater than current balance.");
                }

                user.Balance = request.Balance;
                user.Email = request.Email;
                user.Password = request.Password;
                user.Username = request.Username;

                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.UserRepository.SaveAsync();

                return new ServiceResult<bool>(Common.Constant.StatusCode.SuccessStatusCode, "Update user successfully.");
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
