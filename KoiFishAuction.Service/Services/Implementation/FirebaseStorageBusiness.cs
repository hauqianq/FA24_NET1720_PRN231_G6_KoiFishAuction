using Firebase.Storage;
using JewelryAuction.Business.Business.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace JewelryAuction.Business.Business.Implementation
{
    public class FirebaseStorageBusiness : IFirebaseStorageBusiness
    {
        private readonly IConfiguration _configuration;

        public FirebaseStorageBusiness(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadJewelryImage(IFormFile image)
        {
            string firebaseBucket = _configuration["Firebase:StorageBucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string filename = Guid.NewGuid().ToString() + "_" + image.FileName;

            var task = firebaseStorage.Child("Jewelries").Child(filename);

            var stream = image.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }
    }
}
