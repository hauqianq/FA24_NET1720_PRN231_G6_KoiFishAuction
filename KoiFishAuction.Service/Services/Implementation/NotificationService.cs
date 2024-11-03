using KoiFishAuction.Common;
using KoiFishAuction.Common.Helpers;
using KoiFishAuction.Common.RequestModels.Notification;
using KoiFishAuction.Common.ViewModels.AuctionHistory;
using KoiFishAuction.Common.ViewModels.Notification;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KoiFishAuction.Service.Services.Implementation; 

public interface INotificationService {
    Task<ServiceResult<PagedResponse<NotificationViewModel>>> GetAllNotifications(GetNotificationsRequestModel request);
    Task<ServiceResult<NotificationViewModel>> GetNotificationById(int NotiId);
    Task<ServiceResult<int>> AddNotification(CreateNotificationRequestModel request);
    Task<ServiceResult<int>> UpdateNotification(int id, UpdateNotificationRequestModel request);
    Task<ServiceResult<bool>> DeleteNotification(int NotiId);
}

public class NotificationService : INotificationService {
    private readonly UnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResult<PagedResponse<NotificationViewModel>>> GetAllNotifications(GetNotificationsRequestModel request) {
        var result = await _unitOfWork.NotificationRepository.GetAllNotifications(
            GetPredicate(request),
            GetOrderBy(request),
            n => n.Include(x => x.User)
                .Include(x => x.Sender)
                .Include(x => x.Bid)
                .Include(x => x.Item)
            );

        if (!result.Any()) {
            return new ServiceResult<PagedResponse<NotificationViewModel>>(Constant.StatusCode.FailedStatusCode, Constant.StatusCode.FAIL_READ_MSG);
        }

        var responseList = result.Select(n => new NotificationViewModel {
            Id = n.Id,
            Message = n.Message,
            Type = n.Type,
            IsRead = (bool)n.IsRead!,
            Date = n.Date,
            BidId = n.BidId,
            Remarks = n.Remarks,    
            FishName = n.Item.Name,
            FishAge = n.Item.Age,
            FishPrice = n.Item.CurrentPrice,
            SenderUsername = n.Sender.Username,
            Username = n.Sender.Username,
        });

        var resultPagination = new PagedList<NotificationViewModel>(
            responseList,
            responseList.Count(),
            request.PageNumber,
            request.PageSize);
        return new ServiceResult<PagedResponse<NotificationViewModel>>(Constant.StatusCode.SuccessStatusCode, Constant.StatusCode.SUCCESS_READ_MSG,
            PagedResponse<NotificationViewModel>.CreateResponse(resultPagination));
    }

    public async Task<ServiceResult<int>> AddNotification(CreateNotificationRequestModel request) {
        try {
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));

            var noti = new Notification() {
                Sender = sender,
                UserId = request.UserId,
                ItemId = request.ItemId,
                Message = request.Message,
                Date = DateTime.Now,
                IsRead = false,
                BidId = request.BidId, 
                Remarks = request.Remarks
            };

            await _unitOfWork.NotificationRepository.CreateAsync(noti);
            await _unitOfWork.AuctionSessionRepository.SaveAsync();

            return new ServiceResult<int>(Constant.StatusCode.SuccessStatusCode, "Create a successful auction.", noti.Id);
        }
        catch (Exception e) {
            return new ServiceResult<int>(Constant.StatusCode.FailedStatusCode, e.Message);
        }
    }

    public async Task<ServiceResult<NotificationViewModel>> GetNotificationById(int NotiId) {
        try {
            var notification = await _unitOfWork.NotificationRepository
                .GetNotificationById(x => x.Id == NotiId,
                x => x.Include(x => x.Bid)
                    .Include(x => x.Sender)
                    .Include(x => x.User)
                    .Include(x => x.Item));

            if (notification == null) {
                return new ServiceResult<NotificationViewModel>(Constant.StatusCode.FailedStatusCode, "The notification does not exist.");
            }

            var result = new NotificationViewModel {
                Id = notification.Id,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = (bool)notification.IsRead!,
                Date = notification.Date,
                BidId = notification.BidId,
                Remarks = notification.Remarks,
                FishName = notification.Item.Name,
                FishAge = notification.Item.Age,
                FishPrice = notification.Item.CurrentPrice,
                SenderUsername = notification.Sender.Username,
                Username = notification.Sender.Username,
            };

            return new ServiceResult<NotificationViewModel>(Constant.StatusCode.SuccessStatusCode, result);

        }
        catch (Exception e) {
            return new ServiceResult<NotificationViewModel>(Constant.StatusCode.FailedStatusCode, e.Message);
        }
    }

    public async Task<ServiceResult<int>> UpdateNotification(int id, UpdateNotificationRequestModel request) {
        try {
            if (id != request.Id) {
                return new ServiceResult<int>(Constant.StatusCode.FailedStatusCode);    
            }

            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
            if (notification == null) {
                return new ServiceResult<int>(Constant.StatusCode.FailedStatusCode);
            }

            notification.Message = request.Message;
            notification.Type = request.Type;
            notification.IsRead = request.IsRead;
            notification.Remarks = request.Remarks;

            await _unitOfWork.NotificationRepository.UpdateAsync(notification);
            await _unitOfWork.NotificationRepository.SaveAsync();

            return new ServiceResult<int>(Constant.StatusCode.SuccessStatusCode, "Successfully changed notification information.");
        }
        catch (Exception e) {
            return new ServiceResult<int>(Constant.StatusCode.FailedStatusCode, e.Message);
        }
    }

    public async Task<ServiceResult<bool>> DeleteNotification(int NotiId) {
        try {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(NotiId);
            if (notification == null) {
                return new ServiceResult<bool>(Constant.StatusCode.FailedStatusCode, "This notification does not exist.");
            }

            await _unitOfWork.NotificationRepository.RemoveAsync(notification);
            await _unitOfWork.NotificationRepository.SaveAsync();

            return new ServiceResult<bool>(Constant.StatusCode.SuccessStatusCode, "Delete notification successful.");
        }
        catch (Exception e) {
            return new ServiceResult<bool>(Constant.StatusCode.FailedStatusCode, e.Message);
        }
    }

    private Expression<Func<Notification, bool>> GetPredicate(GetNotificationsRequestModel request) {
        Expression<Func<Notification, bool>>? predicate = default!;

        if (!string.IsNullOrEmpty(request.SearchTerm)) {
            var key = request.SearchTerm.Trim();

            predicate = predicate.AndAlso(u =>
                u.Message.Contains(key) ||
                u.Remarks.Contains(key) ||
                u.Item.Name.Contains(key) ||
                u.User.Username.Contains(key) ||
                u.Sender.Username.Contains(key));
        }

        if (!string.IsNullOrEmpty(request.FishAgeRange) && request.FishAgeRange != "All") {
            switch (request.FishAgeRange) {
                case "age <= 1":
                    predicate = predicate.AndAlso(x => x.Item.Age <= 1);
                    break;
                case "1 < age < 3":
                    predicate = predicate.AndAlso(x => x.Item.Age > 1 && x.Item.Age < 3);
                    break;
                default:
                    predicate = predicate.AndAlso(x => x.Item.Age >= 3);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(request.Type)) {
            predicate = predicate.AndAlso(x => x.Type == request.Type);
        }

        if (!string.IsNullOrEmpty(request.Message)) {
            predicate = predicate.AndAlso(x => x.Message == request.Message);
        }

        if (!string.IsNullOrEmpty(request.FishPriceRange) && request.FishPriceRange != "All") {
            switch (request.FishPriceRange) {
                case "price <= 1.000.000":
                    predicate = predicate.AndAlso(x => x.Item.CurrentPrice <= 1000000);
                    break;
                case "1.000.000 < price < 5.000.000":
                    predicate = predicate.AndAlso(x => x.Item.CurrentPrice > 1000000 && x.Item.CurrentPrice < 5000000);
                    break;
                default:
                    predicate = predicate.AndAlso(x => x.Item.CurrentPrice >= 5000000);
                    break;
            }
        }

        if (request.StartDate != null) {
            predicate = predicate.AndAlso(x => x.Date >= DateTimeExtensions.StartOfDay((DateTime)request.StartDate) &&
                x.Date <= DateTimeExtensions.EndOfDay(request.EndDate));
        }

        return predicate;
    }

    private Func<IQueryable<Notification>, IOrderedQueryable<Notification>> GetOrderBy(GetNotificationsRequestModel request) {
        return request.SortOrder switch {
            "name_desc" => query => query.OrderByDescending(x => x.User.Username),
            "Price" => query => query.OrderBy(x => x.Item.CurrentPrice),
            "price_desc" => query => query.OrderByDescending(x => x.Item.CurrentPrice),
            "CreatedTime" => query => query.OrderBy(x => x.Date),
            "create_desc" => query => query.OrderByDescending(x => x.Date),
            //"UpdatedTime" => query => query.OrderBy(x => x.UpdatedTime),
            //"update_desc" => query => query.OrderByDescending(x => x.UpdatedTime),
            _ => query => query.OrderBy(x => x.User.Username),
        };
    }
}

    

