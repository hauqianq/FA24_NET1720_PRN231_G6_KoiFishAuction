using KoiFishAuction.Common.RequestModels.Bid;
using KoiFishAuction.Common.ViewModels.Bid;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Extensions;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class BidService : IBidService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BidService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<List<BidViewModel>>> GetAllBidForAuctionSessionAsync(int auctionSessionId)
        {
            try
            {
                var auctionSession = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(auctionSessionId);
                if (auctionSession == null)
                {
                    return new ServiceResult<List<BidViewModel>>(KoiFishAuction.Common.Constant.StatusCode.FailedStatusCode, "Auction session not found.");
                }

                var bids = await _unitOfWork.BidRepository.GetBidsByAuctionSessionIdAsync(auctionSessionId);
                var bidViewModels = bids.Select(b => new BidViewModel
                {
                    Amount = b.Amount,
                    Note = b.Note,
                    Timestamp = b.Timestamp,
                    Currency = b.Currency,
                    Location = b.Location,
                    IsWinning = b.IsWinning
                }).ToList();

                return new ServiceResult<List<BidViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, bidViewModels);
            }
            catch (Exception e)
            {
                return new ServiceResult<List<BidViewModel>>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> PlaceBidAsync(CreateBidRequestModel request)
        {
            try
            {
                var auction = await _unitOfWork.AuctionSessionRepository.GetAuctionSessionByIdAsync(request.AuctionSessionId);
                if (auction == null)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "This auction does not exist.");
                }

                var bidder = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));
                if (request.Amount > bidder.Balance)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "Your account balance is not enough to participate in this auction.");
                }


                if (request.Amount <= auction.KoiFish.CurrentPrice)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "The price paid must be at least equal to the starting price.");
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

                return new ServiceResult<bool>(Common.Constant.StatusCode.SuccessStatusCode, "Bidding for this jewelry was successful.");
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
