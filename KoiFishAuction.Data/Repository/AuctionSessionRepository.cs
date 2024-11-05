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

        public async Task<List<AuctionSession>> GetOngoingAuctionSessionAsync()
        {
            return await _context.AuctionSessions.Include(a => a.Winner)
                                                .Include(a => a.Creator)
                                                .Include(a => a.KoiFish).ThenInclude(k => k.KoiImages)
                                                .Include(a => a.Bids)
                                                .Where(a => a.Status == (int)AuctionSessionStatus.Opening)
                                                .ToListAsync();
        }

        public async Task<List<AuctionSession>> GetAuctionSessionForUserAsync(int userid)
        {
            var auctions = await _context.AuctionSessions.Include(a => a.Winner)
                                                .Include(a => a.Creator)
                                                .Include(a => a.KoiFish).ThenInclude(k => k.KoiImages)
                                                .Include(a => a.Bids)
                                                .Where(a => a.CreatorId == userid)
                                                .ToListAsync();

            foreach (var auction in auctions)
            {
                if (auction.EndTime <= DateTime.Now && auction.Status != (int)AuctionSessionStatus.Closed)
                {
                    if (auction.Bids.Any())
                    {
                        await UpdateAuctionSessionStatusAsync(auction.Id, (int)AuctionSessionStatus.Closed);
                    }
                }
            }

            return auctions;
        }


        public async Task<AuctionSession> GetAuctionSessionByIdAsync(int id)
        {
            var result = await _context.AuctionSessions.Include(a => a.KoiFish).ThenInclude(KoiFish => KoiFish.KoiImages)
                                            .Include(a => a.Winner)
                                            .Include(a => a.Creator)
                                            .Include(a => a.Bids)
                                            .FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        private async Task SetWinnerForAuctionSession(int winnerId, int auctionSessionId)
        {
            var auctionSession = await _context.AuctionSessions.FirstOrDefaultAsync(auc => auc.Id == auctionSessionId);
            auctionSession.WinnerId = winnerId;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuctionSessionStatusAsync(int id, int status)
        {
            var auction = await _context.AuctionSessions.FirstOrDefaultAsync(a => a.Id == id);
            if (auction != null)
            {
                auction.Status = status;
                if (status == (int)AuctionSessionStatus.Closed)
                {
                    var winner = await _unitOfWork.BidRepository.FindHighestBidder(id);
                    await SetWinnerForAuctionSession(winner.Id, id);
                }
                _context.AuctionSessions.Update(auction);
                await _unitOfWork.AuctionSessionRepository.SaveAsync();
            }
        }
    }
}
