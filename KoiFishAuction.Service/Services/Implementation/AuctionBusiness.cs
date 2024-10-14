using JewelryAuction.Business.RequestModels.Auction;
using JewelryAuction.Business.RequestModels.Bid;
using JewelryAuction.Business.ViewModels;
using JewelryAuction.Data;
using JewelryAuction.Data.Models;
using KoiFishAuction.Service.Services.Interface;

namespace JewelryAuction.Business.Business.Implementation
{
    public class AuctionBusiness : IAuctionSessionService
    {
        private readonly UnitOfWork _unitOfWork;
        public AuctionBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JewelryAuctionResult> ChangeAuctionStatusAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(id);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                if (auction.Status == Common.Constant.AuctionStatus.Closed)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This auction has already been closed.");
                }

                auction.Status = Common.Constant.AuctionStatus.Closed;
                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "The auction has been closed successfully.");
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> CreateAuctionAsync(CreateAuctionRequestModel request, int ownerid)
        {
            try
            {
                if (request.StartTime >= request.EndTime)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                var status = request.StartTime > DateTime.Now ? Common.Constant.AuctionStatus.Future : Common.Constant.AuctionStatus.Opening;

                var auction = new Auction
                {
                    JewelryId = request.JewelryId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Status = status,
                    CreatorId = ownerid
                };

                await _unitOfWork.AuctionRepository.CreateAsync(auction);
                await _unitOfWork.AuctionRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Create a successful auction.", auction);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> GetAuctionByIdAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(id);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The auction does not exist.");
                }

                var result = new AuctionViewModel()
                {
                    AuctionId = auction.AuctionId,
                    JewelryName = auction.Jewelry.Name,
                    JewelryId = auction.JewelryId,
                    Price = auction.Jewelry.CurrentPrice,
                    ImageUrl = auction.Jewelry.ImageUrl,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    Status = auction.Status,
                    WinnerUsername = auction.Winner != null ? auction.Winner.Username : "There is no winner for this auction yet."
                };

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, result);

            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> GetAuctionsForUserAsync(int userid)
        {
            try
            {

                var data = await _unitOfWork.AuctionRepository.GetAuctionsForUserAsync(userid);

                if (data.Count == 0)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "This user does not have any auctions.");
                }
                else
                {
                    var result = data.Select(auction => new AuctionViewModel()
                    {
                        AuctionId = auction.AuctionId,
                        JewelryId = auction.JewelryId,
                        JewelryName = auction.Jewelry.Name,
                        Price = auction.Jewelry.CurrentPrice,
                        StartTime = auction.StartTime,
                        EndTime = auction.EndTime,
                        Status = auction.Status,
                        WinnerUsername = auction.Winner != null ? auction.Winner.Username : "There is no winner for this auction yet.",
                        ImageUrl = auction.Jewelry.ImageUrl
                    }).ToList();

                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
        public async Task<JewelryAuctionResult> GetOngoingAuctionsAsync(string search = null)
        {
            try
            {

                var data = await _unitOfWork.AuctionRepository.GetOngoingAuctionsAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(a => a.Jewelry.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (data.Count == 0)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "There aren't any auctions.");
                }
                else
                {
                    var result = data.Select(auction => new AuctionViewModel()
                    {
                        AuctionId = auction.AuctionId,
                        JewelryId = auction.JewelryId,
                        JewelryName = auction.Jewelry.Name,
                        Price = auction.Jewelry.CurrentPrice,
                        StartTime = auction.StartTime,
                        EndTime = auction.EndTime,
                        Status = auction.Status,
                        WinnerUsername = auction.Winner != null ? auction.Winner.Username : "There is no winner for this auction yet.",
                        ImageUrl = auction.Jewelry.ImageUrl
                    }).ToList();

                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> CanUpdateAuctionAsync(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(id);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                if (auction.Status == Common.Constant.AuctionStatus.Opening)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "You cannot change auction information while it is in progress.");
                }

                if (auction.Status == Common.Constant.AuctionStatus.Closed)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "You cannot change the information of an auction once it has ended.");
                }

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> PlaceBidAsync(CreateBidRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(request.AuctionId);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                var bidder = await _unitOfWork.UserRepository.GetUserByIdAsync(request.BidderId);
                if (request.Amount > bidder.Balance)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "Your account balance is not enough to participate in this auction.");
                }


                if (request.Amount <= auction.Jewelry.CurrentPrice)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The price paid must be at least equal to the starting price.");
                }

                var bid = new Bid
                {
                    AuctionId = request.AuctionId,
                    BidderId = request.BidderId,
                    Amount = request.Amount,
                    BidTime = DateTime.UtcNow
                };

                bidder.Balance -= bid.Amount;
                await _unitOfWork.UserRepository.SaveAsync();

                var result = await _unitOfWork.JewelryRepository.UpdateJewelryCurentPriceAsync(auction.JewelryId, request.Amount);
                if (result)
                {
                    await _unitOfWork.BidRepository.CreateAsync(bid);
                    await _unitOfWork.BidRepository.SaveAsync();

                    await _unitOfWork.AuctionRepository.SaveAsync();
                }

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Bidding for this jewelry was successful.");
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }


        public async Task<JewelryAuctionResult> SetAuctionWinnerAsync(int auctionId, int winnerId)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(auctionId);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode);
                }

                var winner = await _unitOfWork.UserRepository.GetByIdAsync(winnerId);
                if (winner == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode);
                }

                auction.WinnerId = winnerId;
                await _unitOfWork.AuctionRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> UpdateAuctionAsync(UpdateAuctionRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(request.AuctionId);
                if (auction == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode);
                }

                if (request.StartTime <= DateTime.Now)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The auction creation date must be after to day.");
                }

                if (request.StartTime >= request.EndTime)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The auction end time must be after the start time.");
                }

                auction.StartTime = request.StartTime;
                auction.EndTime = request.EndTime;
                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                await _unitOfWork.AuctionRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Successfully changed auction information.");
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
