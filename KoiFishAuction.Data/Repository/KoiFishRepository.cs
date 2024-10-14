using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class KoiFishRepository : GenericRepository<KoiFish>
    {
        //private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;

        //public KoiFishRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        //{
        //    _context = context;
        //}
        //public async Task<bool> IsKoiInAuction(int id)
        //{
        //    var result = await _context.AuctionSessions.AnyAsync(a => a.ItemId == id);
        //    return result;
        //}
        //public async Task<KoiFish> GetKoiFishByIdAsync(int id)
        //{
        //    var result = await _context.KoiFishes.Include(j => j.AuctionHistories)
        //                                            .Include(j => j.AuctionSessions)
        //                                            .Include(j => j.Bids)
        //                                            .Include(j => j.Notifications)
        //                                            .FirstOrDefaultAsync(j => j.Id == id);
        //    return result;
        //}

        //public async Task<List<KoiFish>> GetAllKoiFishesAsync(int sellerid)
        //{
        //    var result = await _context.KoiFishes.Include(j => j.AuctionHistories)
        //                                            .Include(j => j.AuctionSessions)
        //                                            .Include(j => j.Bids)
        //                                            .Include(j => j.Notifications)
        //                                            .ToListAsync();
        //    return result;
        //}
    }
}
