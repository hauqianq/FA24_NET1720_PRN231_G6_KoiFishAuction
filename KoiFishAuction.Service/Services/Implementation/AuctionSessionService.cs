using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.Bid;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Enumerrations;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Extensions;
using KoiFishAuction.Service.Services;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace JewelryAuction.Business.Business.Implementation
{
    public class AuctionSessionService : IAuctionSessionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuctionSessionService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<bool>> ChangeAuctionStatusAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                if (auction.Status == (int)AuctionSessionStatus.Opening)
                {
                    return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "This auction has already been closed.");
                }

                auction.Status = (int)AuctionSessionStatus.Closed;
                await _unitOfWork.AuctionSessionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "The auction has been closed successfully.");
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<int>> CreateAuctionAsync(CreateAuctionSessionRequestModel request)
        {
            try
            {
                if (request.StartTime >= request.EndTime)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                var status = request.StartTime > DateTime.Now ? (int)AuctionSessionStatus.Future : (int)AuctionSessionStatus.Opening;

                var creator = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));

                var auctionSession = new AuctionSession()
                {
                    Name = request.Name,
                    Note = request.Note,
                    KoiFishId = request.KoiFishId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Status = status,
                    MinIncrement = request.MinIncrement,
                    CreatorId = creator.Id
                };

                await _unitOfWork.AuctionSessionRepository.CreateAsync(auctionSession);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "Create a successful auction.", auctionSession.Id);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionByIdAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult<AuctionSessionDetailViewModel>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                var bids = await _unitOfWork.BidRepository.GetBidsByAuctionSessionIdAsync(auction.Id);

                var result = new AuctionSessionDetailViewModel()
                {

                    Id = auction.Id,
                    Name = auction.Name,
                    Note = auction.Note,
                    KoiFishName = auction.KoiFish.Name,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    Status = ((AuctionSessionStatus)auction.Status).ToString(),
                    MinIncrement = auction.MinIncrement,
                    Price = auction.KoiFish.CurrentPrice,
                    Images = auction.KoiFish.KoiImages.Select(koiImage => koiImage.ImageUrl).ToList(),
                    WinnerUsername = auction.Winner != null ? auction.Winner.Username : "There is no winner for this auction yet.",
                    BidViewModels = bids.Select(bid => new BidViewModel()
                    {
                        BidderName = bid.Bidder.Username,
                        Amount = bid.Amount,
                        Timestamp = bid.Timestamp,
                        Currency = bid.Currency,
                        IsWinning = bid.IsWinning,
                        Location = bid.Location,
                        Note = bid.Note
                    }).ToList()
                };

                return new ServiceResult<AuctionSessionDetailViewModel>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);

            }
            catch (Exception e)
            {
                return new ServiceResult<AuctionSessionDetailViewModel>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionsForUserAsync()
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));

                var data = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionForUserAsync(user.Id);

                if (data.Count == 0)
                {
                    return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "This user does not have any auctions.");
                }
                else
                {
                    var result = data.Select(auc => new AuctionSessionViewModel()
                    {
                        Id = auc.Id,
                        Name = auc.Name,
                        KoiFishName = auc.KoiFish.Name,
                        StartTime = auc.StartTime,
                        EndTime = auc.EndTime,
                        Status = auc.Status.ToString(),
                        Price = auc.KoiFish.CurrentPrice,
                    }).ToList();

                    return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionsAsync(string search = null)
        {
            try
            {

                var data = await _unitOfWork.AuctionSessionRepository.GetOngoingAuctionSessionAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(a => a.KoiFish.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (data.Count == 0)
                {
                    return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "There aren't any auctions.");
                }
                else
                {
                    var result = data.Select(auc => new AuctionSessionViewModel()
                    {
                        Id = auc.Id,
                        Name = auc.Name,
                        KoiFishName = auc.KoiFish.Name,
                        StartTime = auc.StartTime,
                        EndTime = auc.EndTime,
                        Status = ((AuctionSessionStatus)auc.Status).ToString(),
                        Price = auc.KoiFish.CurrentPrice,
                        Image = auc.KoiFish.KoiImages.Select(KoiFish => KoiFish.ImageUrl).FirstOrDefault()
                    }).ToList();

                    return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<List<AuctionSessionViewModel>>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> CanUpdateAuctionAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                if (auction.Status == (int)AuctionSessionStatus.Opening)
                {
                    return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "You cannot change auction information while it is in progress.");
                }

                if (auction.Status == (int)AuctionSessionStatus.Closed)
                {
                    return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "You cannot change the information of an auction once it has ended.");
                }

                return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<int>> SetAuctionWinnerAsync(int auctionId)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(auctionId);
                if (auction == null)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                var winner = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));
                if (winner == null)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                auction.WinnerId = winner.Id;
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<int>> UpdateAuctionAsync(UpdateAuctionSessionRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.Id);
                if (auction == null)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                if (request.StartTime <= DateTime.Now)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction creation date must be after to day.");
                }

                if (request.StartTime >= request.EndTime)
                {
                    return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                auction.StartTime = request.StartTime;
                auction.EndTime = request.EndTime;
                await _unitOfWork.AuctionSessionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "Successfully changed auction information.");
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
