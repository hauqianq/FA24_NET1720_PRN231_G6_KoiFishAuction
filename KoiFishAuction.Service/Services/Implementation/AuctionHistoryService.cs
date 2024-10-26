using System.Linq.Expressions;
using KoiFishAuction.Common;
using KoiFishAuction.Common.RequestModels.AuctionHistory;
using KoiFishAuction.Common.ViewModels.AuctionHistory;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Service.Services.Implementation;

public class AuctionHistoryService : IAuctionHistoryService
{
    private readonly UnitOfWork _unitOfWork;

    public AuctionHistoryService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PagedResponse<AuctionHistoryViewModel>>> GetAllAuctionHistory(AuctionHistoryParams auctionHistoryParams)
    {
        var result = await _unitOfWork.AuctionHistoryRepository.GetAllAuctionHistory(
            GetPredicate(auctionHistoryParams),
            GetOrderBy(auctionHistoryParams),
            ah => ah
                .Include(ah => ah.Owner)
                .Include(ah => ah.Winner)
                .Include(ah => ah.AuctionSession)
                .ThenInclude(a => a.KoiFish));
        if (!result.Any())
        {
            return new ServiceResult<PagedResponse<AuctionHistoryViewModel>>(Constant.StatusCode.FailedStatusCode, Constant.StatusCode.FAIL_READ_MSG);
        }
        
        var responseList = result.Select(ah => new AuctionHistoryViewModel
        {
            Id = ah.Id,
            DeliveryStatus = ah.DeliveryStatus,
            FeedbackStatus = ah.FeedbackStatus,
            PaymentStatus = ah.PaymentStatus,
            WinningDate = ah.WinningDate,
            Remarks = ah.Remarks,
            WinningAmount = ah.WinningAmount,
            FishName = ah.AuctionSession.KoiFish.Name,
            OwnerName = ah.Owner.FullName,
            WinnerName = ah.Winner.FullName,
            AuctionSessionStatus = ah.AuctionSession.Status,
            AuctionSessionEndTime = ah.AuctionSession.EndTime,
            AuctionSessionStartTime = ah.AuctionSession.StartTime
        });
        
        var resultPagination = new PagedList<AuctionHistoryViewModel>(
            responseList,
            responseList.Count(),
            auctionHistoryParams.PageNumber,
            auctionHistoryParams.PageSize);
        return new ServiceResult<PagedResponse<AuctionHistoryViewModel>>(Constant.StatusCode.SuccessStatusCode, Constant.StatusCode.SUCCESS_READ_MSG,
            PagedResponse<AuctionHistoryViewModel>.CreateResponse(resultPagination));
    }

    public Expression<Func<AuctionHistory, bool>> GetPredicate(AuctionHistoryParams auctionHistoryParams)
    {
        Expression<Func<AuctionHistory, bool>> predicate = default;

        if (!string.IsNullOrEmpty(auctionHistoryParams.DeliveryStatus))
        {
            predicate = ah => ah.DeliveryStatus.ToLower() == auctionHistoryParams.DeliveryStatus.ToLower();
        }

        if (!string.IsNullOrEmpty(auctionHistoryParams.FeedbackStatus))
        {
            predicate = ah => ah.FeedbackStatus.ToLower() == auctionHistoryParams.FeedbackStatus.ToLower();
        }

        if (!string.IsNullOrEmpty(auctionHistoryParams.PaymentStatus))
        {
            predicate = ah => ah.PaymentStatus.ToLower() == auctionHistoryParams.PaymentStatus.ToLower();
        }

        return predicate ?? (a => true);
    }

    public Func<IQueryable<AuctionHistory>, IOrderedQueryable<AuctionHistory>> GetOrderBy
        (AuctionHistoryParams auctionHistoryParams) => auctionHistoryParams.OrderBy?.ToLower() switch
    {
        "_winningBid" => ah => ah.OrderBy(ah => ah.WinningAmount),
        "_dateAsc" => ah => ah.OrderBy(ah => ah.WinningDate),
        "_dateDesc" => ah => ah.OrderByDescending(ah => ah.WinningDate),
        _ => ah => ah.OrderBy(ah => ah.Id)
    };
}