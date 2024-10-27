using KoiFishAuction.Data.Base;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.Data.Repository
{
    public class KoiImageRepository : GenericRepository<KoiImage>
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;

        public KoiImageRepository(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }

        public async Task<KoiImage> GetImageByUrlAsync(string imageUrl)
        {
            var image = await _context.KoiImages.FirstOrDefaultAsync(x => x.ImageUrl == imageUrl);
            return image;
        }
    }
}
