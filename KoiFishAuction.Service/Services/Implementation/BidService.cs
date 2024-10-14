using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.Bid;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Services.Interface;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class BidService : IBidService
    {
        private readonly UnitOfWork _unitOfWork;
        public BidService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetAllBidForAuctionSessionAsync(int auctionSessionId)
        {
            try
            {
                var auctionSession = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(auctionSessionId);
                if (auctionSession == null)
                {
                    return new ServiceResult(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "Auction session not found.");
                }

                var bids = await _unitOfWork.BidRepository.GetBidsByAuctionSessionIdAsync(auctionSessionId);
                var bidViewModels = bids.Select(b => new BidViewModel
                {
                    Id = b.Id,
                    AuctionSessionId = b.AuctionSessionId,
                    BidderId = b.BidderId,
                    Amount = b.Amount,
                    Note = b.Note,
                    Timestamp = b.Timestamp,
                    Currency = b.Currency,
                    Location = b.Location,
                    IsWinning = b.IsWinning
                }).ToList();

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, bidViewModels);
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> GetBidByIdAsync(int id)
        {
            try
            {
                var bid = await _unitOfWork.BidRepository.GetBidByIdAsync(id);
                if (bid == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "Bid not found.");
                }

                var bidViewModel = new BidViewModel
                {
                    Id = bid.Id,
                    AuctionSessionId = bid.AuctionSessionId,
                    BidderId = bid.BidderId,
                    Amount = bid.Amount,
                    Note = bid.Note,
                    Timestamp = bid.Timestamp,
                    Currency = bid.Currency,
                    Location = bid.Location,
                    IsWinning = bid.IsWinning
                };

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, bidViewModel);
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> PlaceBidAsync(CreateBidRequestModel request, int bidderid)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.AuctionSessionId);
                if (auction == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                var bidder = await _unitOfWork.UserRepository.GetUserByIdAsync(bidderid);
                if (request.Amount > bidder.Balance)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "Your account balance is not enough to participate in this auction.");
                }


                if (request.Amount <= auction.KoiFish.CurrentPrice)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "The price paid must be at least equal to the starting price.");
                }

                var bid = new Bid
                {
                    AuctionSessionId = request.AuctionSessionId,
                    BidderId = bidder.Id,
                    Amount = request.Amount,
                    Note = request.Note,
                    Timestamp = DateTime.UtcNow,
                    Currency = request.Currency,
                    Location = request.Location,
                    IsWinning = false
                };
                bidder.Balance -= bid.Amount;
                await _unitOfWork.UserRepository.SaveAsync();

                var result = await _unitOfWork.KoiFishRepository.UpdateKoiFishCurentPriceAsync(auction.KoiFishId, request.Amount);
                if (result)
                {
                    await _unitOfWork.BidRepository.CreateAsync(bid);
                    await _unitOfWork.BidRepository.SaveAsync();

                    await _unitOfWork.AuctionSessionRepository.SaveAsync();
                }

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, "Bidding for this jewelry was successful.");
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
