using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace KoiFishAuction.Data.Repository;

public class NotificationRepository : GenericRepository<Notification>
{
    public async Task<List<Notification>> GetAllNotifications(
            Expression<Func<Notification, bool>>? predicate,
            Func<IQueryable<Notification>, IOrderedQueryable<Notification>>? orderBy,
            Func<IQueryable<Notification>, IIncludableQueryable<Notification, object?>>? includeEntity,
            bool tracking = false) {

        IQueryable<Notification> query = tracking ? _context.Notifications.AsQueryable() : _context.Notifications.AsNoTracking();
        
        if (includeEntity != null) {
            query = includeEntity(query);
        }

        if (predicate != null) {
            query = query.Where(predicate);
        }
        if (orderBy != null) {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<Notification?> GetNotificationById(
            Expression<Func<Notification, bool>>? predicate,
            Func<IQueryable<Notification>, IIncludableQueryable<Notification, object?>>? includeEntity,
            bool tracking = false) {

        IQueryable<Notification> query = tracking ? _context.Notifications.AsQueryable() : _context.Notifications.AsNoTracking();

        if (includeEntity != null) {
            query = includeEntity(query);
        }

        if (predicate != null) {
            query = query.Where(predicate);
        }

        return await query.FirstOrDefaultAsync();
    }
}
