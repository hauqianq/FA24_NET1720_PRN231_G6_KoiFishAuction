using Microsoft.AspNetCore.Http;

namespace KoiFishAuction.Service.Services.Interface
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadKoiFishImage(IFormFile image);
        Task DeleteKoiFishImage(string imageUrl);
    }
}
