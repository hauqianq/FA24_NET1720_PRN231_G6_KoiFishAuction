﻿using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class BidRepository : GenericRepository<Bid>
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;
        public BidRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _context.Bids.Include(b=>b.Bidder)
                                    .Include(b => b.AuctionSession)
                                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<Bid>> GetBidsByAuctionSessionIdAsync(int auctionId)
        {
            return await _context.Bids.Include(b => b.Bidder)
                                               .Include(b => b.AuctionSession)
                                               .Where(b => b.Id == auctionId).ToListAsync();
        }
    }
}