using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class KoiFishRepository : GenericRepository<KoiFish>
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;

        public KoiFishRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }
        public async Task<bool> IsKoiInAuction(int id)
        {
            var result = await _context.AuctionSessions.AnyAsync(a => a.KoiFishId == id);
            return result;
        }
        public async Task<KoiFish> GetKoiFishByIdAsync(int id)
        {
            var result = await _context.KoiFishes.Include(koiFish => koiFish.AuctionHistories)
                                                    .Include(koiFish => koiFish.AuctionSessions)
                                                    .Include(koiFish => koiFish.Bids)
                                                    .Include(koiFish => koiFish.Notifications)
                                                    .FirstOrDefaultAsync(koiFish => koiFish.Id == id);
            return result;
        }

        public async Task<List<KoiFish>> GetAllKoiFishesAsync(int sellerid)
        {
            var result = await _context.KoiFishes.Include(koiFish => koiFish.AuctionHistories)
                                                    .Include(koiFish => koiFish.AuctionSessions)
                                                    .Include(koiFish => koiFish.Bids)
                                                    .Include(koiFish => koiFish.Notifications)
                                                    .Include(KoiFish => KoiFish.Seller)
                                                    .Where(koiFish => koiFish.SellerId == sellerid)
                                                    .ToListAsync();
            return result;
        }

        public async Task<bool> UpdateKoiFishCurentPriceAsync(int id, decimal price)
        {
            var koiFishewelry = await _context.KoiFishes.FirstOrDefaultAsync(koiFish => koiFish.Id == id);
            if (koiFishewelry == null)
            {
                return false;
            }

            koiFishewelry.CurrentPrice = price;

            _context.KoiFishes.Update(koiFishewelry);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
