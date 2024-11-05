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

namespace KoiFishAuction.Service.Services.Implementation
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

        public async Task<ServiceResult<int>> CreateAuctionSessionAsync(CreateAuctionSessionRequestModel request)
        {
            try
            {
                if (request.StartTime >= request.EndTime)
                {
                    return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
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

                return new ServiceResult<int>(Common.Constant.StatusCode.SuccessStatusCode, "Create a successful auction.", auctionSession.Id);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<AuctionSessionDetailViewModel>> GetAuctionSessionByIdAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult<AuctionSessionDetailViewModel>(Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
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

                return new ServiceResult<AuctionSessionDetailViewModel>(Common.Constant.StatusCode.SuccessStatusCode, result);

            }
            catch (Exception e)
            {
                return new ServiceResult<AuctionSessionDetailViewModel>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetAuctionSessionForUserAsync()
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));

                var data = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionForUserAsync(user.Id);

                if (data.Count == 0)
                {
                    return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, "This user does not have any auctions.");
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
                        Status = Enum.IsDefined(typeof(AuctionSessionStatus), auc.Status)
                                ? ((AuctionSessionStatus)auc.Status).ToString()
                                : "Unknown",

                        Price = auc.KoiFish.CurrentPrice,
                        Image = auc.KoiFish.KoiImages.Select(KoiFish => KoiFish.ImageUrl).FirstOrDefault()
                    }).ToList();

                    return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
        public async Task<ServiceResult<List<AuctionSessionViewModel>>> GetOngoingAuctionSessionAsync(string search = null)
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
                    return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, "There aren't any auctions.");
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
                        Status = Enum.IsDefined(typeof(AuctionSessionStatus), auc.Status)
                                ? ((AuctionSessionStatus)auc.Status).ToString()
                                : "Unknown",
                        Price = auc.KoiFish.CurrentPrice,
                        Image = auc.KoiFish.KoiImages.Select(KoiFish => KoiFish.ImageUrl).FirstOrDefault()
                    }).ToList();

                    return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<List<AuctionSessionViewModel>>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        private async Task ValidateAuctionSessionAsync(UpdateAuctionSessionRequestModel request)
        {
            var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.Id);

            if (auction.Status == (int)AuctionSessionStatus.Opening)
            {
                throw new InvalidOperationException("You cannot change auction information while it is in progress.");
            }

            if (auction.Status == (int)AuctionSessionStatus.Closed)
            {
                throw new InvalidOperationException("You cannot change the information of an auction once it has ended.");
            }

            if (request.StartTime <= DateTime.Now)
            {
                throw new InvalidOperationException("The auction creation date must be after today.");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new InvalidOperationException("The auction end time must be after the start time.");
            }
        }


        public async Task<ServiceResult<int>> UpdateAuctionSessionAsync(UpdateAuctionSessionRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.Id);
                if (auction == null)
                {
                    return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                await ValidateAuctionSessionAsync(request);

                auction.StartTime = request.StartTime;
                auction.EndTime = request.EndTime;
                auction.Name = request.Name;
                auction.Note = request.Note;
                auction.MinIncrement = request.MinIncrement;
                auction.Status = request.Status;

                await _unitOfWork.AuctionSessionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<int>(Common.Constant.StatusCode.SuccessStatusCode, "Successfully changed auction information.");
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteAuctionSessionAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.", false);
                }

                if (auction.Status == (int)AuctionSessionStatus.Opening)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "You cannot delete an auction that is in progress.", false);
                }

                if (auction.Status == (int)AuctionSessionStatus.Closed)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "You cannot delete an auction that has ended.", false);
                }

                await _unitOfWork.AuctionSessionRepository.RemoveAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult<bool>(Common.Constant.StatusCode.SuccessStatusCode, "The auction has been deleted successfully.", true);
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, e.Message, false);
            }
        }
    }
}
