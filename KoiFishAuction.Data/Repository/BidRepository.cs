using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class BidRepository
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;
        public BidRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids.Include(b => b.Bidder)
                                               .Include(b => b.AuctionSession)
                                               .Where(b => b.Id == auctionId).ToListAsync();
        }
    }
}
