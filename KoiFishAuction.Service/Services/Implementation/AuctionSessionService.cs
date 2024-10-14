using KoiFishAuction.Common.RequestModels.AuctionSession;
using KoiFishAuction.Common.ViewModels.AuctionSession;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Enumerrations;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Services.Implementation;
using KoiFishAuction.Service.Services.Interface;

namespace JewelryAuction.Business.Business.Implementation
{
    public class AuctionSessionService : IAuctionSessionService
    {
        private readonly UnitOfWork _unitOfWork;
        public AuctionSessionService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> ChangeAuctionStatusAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                if (auction.Status == (int)AuctionStatus.Opening)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "This auction has already been closed.");
                }

                auction.Status = (int)AuctionStatus.Closed;
                await _unitOfWork.AuctionSessionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "The auction has been closed successfully.");
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> CreateAuctionAsync(CreateAuctionSessionRequestModel request, int ownerid)
        {
            try
            {
                if (request.StartTime >= request.EndTime)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                var status = request.StartTime > DateTime.Now ? (int)AuctionStatus.Future : (int)AuctionStatus.Opening;

                var auctionSession = new AuctionSession()
                {
                    Name = request.Name,
                    Note = request.Note,
                    KoiFishId = request.KoiFishId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Status = status,
                    MinIncrement = request.MinIncrement,
                    CreatorId = ownerid
                };

                await _unitOfWork.AuctionSessionRepository.CreateAsync(auctionSession);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "Create a successful auction.", auctionSession);
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> GetAuctionByIdAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                var result = new AuctionSessionViewModel()
                {

                    Id = auction.Id,
                    Name = auction.Name,
                    Note = auction.Note,
                    KoiFishId = auction.KoiFishId,
                    KoiFishName = auction.KoiFish.Name,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    Status = auction.Status.ToString(),
                    MinIncrement = auction.MinIncrement,
                    ImageUrl = auction.KoiFish.ImageUrl,
                    Price = auction.KoiFish.CurrentPrice,
                    WinnerUsername = auction.Winner != null ? auction.Winner.Username : "There is no winner for this auction yet.",
                };

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);

            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> GetAuctionsForUserAsync(int userid)
        {
            try
            {

                var data = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionForUserAsync(userid);

                if (data.Count == 0)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "This user does not have any auctions.");
                }
                else
                {
                    var result = data.Select(auc => new AuctionSessionViewModel()
                    {
                        Id = auc.Id,
                        Name = auc.Name,
                        Note = auc.Note,
                        KoiFishId = auc.KoiFishId,
                        KoiFishName = auc.KoiFish.Name,
                        StartTime = auc.StartTime,
                        EndTime = auc.EndTime,
                        Status = auc.Status.ToString(),
                        MinIncrement = auc.MinIncrement,
                        ImageUrl = auc.KoiFish.ImageUrl,
                        Price = auc.KoiFish.CurrentPrice,
                        WinnerUsername = auc.Winner != null ? auc.Winner.Username : "There is no winner for this auction yet.",
                    }).ToList();

                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
        public async Task<ServiceResult> GetOngoingAuctionsAsync(string search = null)
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
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "There aren't any auctions.");
                }
                else
                {
                    var result = data.Select(auc => new AuctionSessionViewModel()
                    {
                        Id = auc.Id,
                        Name = auc.Name,
                        Note = auc.Note,
                        KoiFishId = auc.KoiFishId,
                        KoiFishName = auc.KoiFish.Name,
                        StartTime = auc.StartTime,
                        EndTime = auc.EndTime,
                        Status = auc.Status.ToString(),
                        MinIncrement = auc.MinIncrement,
                        ImageUrl = auc.KoiFish.ImageUrl,
                        Price = auc.KoiFish.CurrentPrice,
                        WinnerUsername = auc.Winner != null ? auc.Winner.Username : "There is no winner for this auction yet.",
                    }).ToList();

                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> CanUpdateAuctionAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(id);
                if (auction == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                if (auction.Status == (int)AuctionStatus.Opening)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "You cannot change auction information while it is in progress.");
                }

                if (auction.Status == (int)AuctionStatus.Closed)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "You cannot change the information of an auction once it has ended.");
                }

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> SetAuctionWinnerAsync(int auctionId, int winnerId)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(auctionId);
                if (auction == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                var winner = await _unitOfWork.UserRepository.GetByIdAsync(winnerId);
                if (winner == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                auction.WinnerId = winnerId;
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> UpdateAuctionAsync(UpdateAuctionSessionRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.Id);
                if (auction == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode);
                }

                if (request.StartTime <= DateTime.Now)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction creation date must be after to day.");
                }

                if (request.StartTime >= request.EndTime)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                auction.StartTime = request.StartTime;
                auction.EndTime = request.EndTime;
                await _unitOfWork.AuctionSessionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();

                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.SuccessStatusCode, "Successfully changed auction information.");
            }
            catch (Exception e)
            {
                return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
