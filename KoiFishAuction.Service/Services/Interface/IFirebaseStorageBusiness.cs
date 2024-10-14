using Microsoft.AspNetCore.Http;

namespace JewelryAuction.Business.Business.Interface
{
    public interface IFirebaseStorageBusiness
    {
        Task<string> UploadJewelryImage(IFormFile image);
    }
}
