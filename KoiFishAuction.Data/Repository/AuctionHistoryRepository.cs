using System.Linq.Expressions;
using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KoiFishAuction.Data.Repository
{
    public class AuctionHistoryRepository : GenericRepository<AuctionHistory>
    {
        public async Task<List<AuctionHistory>> GetAllAuctionHistory(
            Expression<Func<AuctionHistory, bool>>? predicate,
            Func<IQueryable<AuctionHistory>, IOrderedQueryable<AuctionHistory>>? orderBy,
            Func<IQueryable<AuctionHistory>, IIncludableQueryable<AuctionHistory, object?>>? includeEntity,
            bool tracking = false)
        {
            IQueryable<AuctionHistory> query = tracking ? _context.AuctionHistories.AsQueryable() : _context.AuctionHistories.AsNoTracking();
            if (includeEntity != null)
            {
                query = includeEntity(query);
            }
            
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }
        
    }
}
