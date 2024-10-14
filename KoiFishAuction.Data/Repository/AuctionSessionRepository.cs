using JewelryAuction.Data;
using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Enumerrations;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class AuctionSessionRepository : GenericRepository<AuctionSession>
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;
        private readonly UnitOfWork _unitOfWork;
        public AuctionSessionRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context, UnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AuctionSession>> GetOngoingAuctionsAsync()
        {
            return await _context.AuctionSessions.Include(a => a.Winner)
                                            .Include(a => a.Creator)
                                                .Include(a => a.KoiFish)
                                                .Include(a => a.Bids)
                                            .Where(a => a.Status == (int)AuctionStatus.Open)
                                            .ToListAsync();
        }

        public async Task<List<AuctionSession>> GetAuctionsForUserAsync(int userid)
        {
            var auctions = await _context.AuctionSessions.Where(a => a.CreatorId == userid).ToListAsync();
            foreach (var auction in auctions)
            {
                if (auction.EndTime <= DateTime.Now && auction.Status != (int) AuctionStatus.Closed)
                {
                    await UpdateAuctionStatusAsync(auction.Id, (int)AuctionStatus.Closed);
                }
            }

            return await _context.AuctionSessions.Include(a => a.KoiFish)
                                            .Include(a => a.Winner)
                                            .Include(a => a.Creator)
                                            .Include(a => a.Bids)
                                            .Where(a => a.CreatorId == userid).ToListAsync();
        }

        public async Task<AuctionSession> GetAuctionByIdAsync(int id)
        {
            var result = await _context.AuctionSessions.Include(a => a.KoiFish)
                                            .Include(a => a.Winner)
                                            .Include(a => a.Creator)
                                            .Include(a => a.Bids)
                                                .FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        public async Task SetAuctionWinnerAsync(int auctionId, int winnerId)
        {
            var auction = await _context.AuctionSessions.FirstOrDefaultAsync(a => a.Id == auctionId);
            if (auction != null)
            {
                auction.WinnerId = winnerId;
                _context.AuctionSessions.Update(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();
            }
        }

        public async Task UpdateAuctionStatusAsync(int auctionId, int status)
        {
            var auction = await _context.AuctionSessions.FirstOrDefaultAsync(a => a.Id == auctionId);
            if (auction != null)
            {
                auction.Status = status;
                _context.AuctionSessions.Update(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();
            }
        }
    }
}
