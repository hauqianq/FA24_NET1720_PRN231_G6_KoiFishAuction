using Firebase.Storage;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace JewelryAuction.Business.Business.Implementation
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _configuration;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadKoiFishImage(IFormFile image)
        {
            string firebaseBucket = _configuration["Firebase:StorageBucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string filename = Guid.NewGuid().ToString() + "_" + image.FileName;

            var task = firebaseStorage.Child("KoiFish").Child(filename);

            var stream = image.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }
    }
}
