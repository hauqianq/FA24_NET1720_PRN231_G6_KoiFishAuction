using Firebase.Storage;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace KoiFishAuction.Service.Services.Implementation
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

            var task = firebaseStorage.Child("KoiFishes").Child(filename);

            var stream = image.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }

        public async Task DeleteKoiFishImage(string imageUrl)
        {
            try
            {
                string firebaseBucket = _configuration["Firebase:StorageBucket"];
                var firebaseStorage = new FirebaseStorage(firebaseBucket);

                // Tìm đường dẫn trong Firebase để xóa ảnh
                var storagePath = new Uri(imageUrl).AbsolutePath.Split(new string[] { "/o/" }, StringSplitOptions.None)[1].Split('?')[0];
                await firebaseStorage.Child(storagePath).DeleteAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to delete image from Firebase: " + e.Message);
            }
        }


    }
}
