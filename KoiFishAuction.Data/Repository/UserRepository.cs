using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;

        public UserRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userid)
        {
            var user = await _context.Users.Include(u => u.AuctionHistoryOwners)
                                            .Include(u => u.AuctionHistoryWinners)
                                            .Include(u => u.KoiFishes)
                                            .Include(u => u.Bids)
                                            .Include(u => u.NotificationUsers)
                                            .Include(u => u.Payments)
                                            .FirstOrDefaultAsync(u => u.Id == userid);
            return user;
        }
    }
}
