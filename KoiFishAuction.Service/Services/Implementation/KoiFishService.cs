using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Extensions;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class KoiFishService : IKoiFishService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKoiImageService _koiImageService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public KoiFishService(UnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageBusiness, IHttpContextAccessor httpContextAccessor, IKoiImageService koiImageService, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageBusiness = firebaseStorageBusiness;
            _httpContextAccessor = httpContextAccessor;
            _koiImageService = koiImageService;
            _firebaseStorageService = firebaseStorageService;
        }

        private async Task<List<KoiImage>> AddListImages(List<IFormFile> request)
        {
            List<KoiImage> images = new List<KoiImage>();
            foreach (var image in request)
            {
                var img = new KoiImage()
                {
                    ImageUrl = await _firebaseStorageBusiness.UploadKoiFishImage(image),
                };
                images.Add(img);
            }
            return images;
        }
        public async Task<ServiceResult<int>> CreateKoiFishAsync(CreateKoiFishRequestModel request)
        {
            try
            {
                if (request.StartingPrice <= 0)
                {
                    return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, "The starting price must be greater than 0.");
                }

                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(_httpContextAccessor.GetCurrentUserId()));

                var koiFish = new KoiFish()
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartingPrice = request.StartingPrice,
                    CurrentPrice = request.CurrentPrice,
                    Age = request.Age,
                    Origin = request.Origin,
                    Weight = request.Weight,
                    Length = request.Length,
                    ColorPattern = request.ColorPattern,
                    SellerId = user.Id,
                    KoiImages = await AddListImages(request.Images)
                };

                await _unitOfWork.KoiFishRepository.CreateAsync(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult<int>(Common.Constant.StatusCode.SuccessStatusCode, "Create successful koi fish.", koiFish.Id);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteKoiFishAsync(int id)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
                if (koiFish == null)
                {
                    return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, "This koi fish does not exist.");
                }

                await _unitOfWork.KoiFishRepository.RemoveAsync(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult<bool>(Common.Constant.StatusCode.SuccessStatusCode, "Delete koi fish successful.");
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync(int userId)
        {
            try
            {
                var data = await _unitOfWork.KoiFishRepository.GetAllKoiFishesAsync(userId);
                if (data.Count == 0)
                {
                    return new ServiceResult<List<KoiFishViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, "You do not currently own any koiFish.");
                }
                else
                {
                    var result = data.Select(KoiFish => new KoiFishViewModel()
                    {
                        ColorPattern = KoiFish.ColorPattern,
                        CurrentPrice = KoiFish.CurrentPrice,
                        Id = KoiFish.Id,
                        Name = KoiFish.Name,
                        Origin = KoiFish.Origin,
                    }).ToList();

                    return new ServiceResult<List<KoiFishViewModel>>(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<List<KoiFishViewModel>>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult<KoiFishDetailViewModel>> GetKoiFishByIdAsync(int id)
        {
            try
            {
                var data = await _unitOfWork.KoiFishRepository.GetKoiFishByIdAsync(id);
                if (data != null)
                {

                    var result = new KoiFishDetailViewModel()
                    {
                        Age = data.Age,
                        ColorPattern = data.ColorPattern,
                        CurrentPrice = data.CurrentPrice,
                        Description = data.Description,
                        Id = data.Id,
                        Length = data.Length,
                        Name = data.Name,
                        Origin = data.Origin,
                        SellerUserName = data.Seller.Username,
                        StartingPrice = data.StartingPrice,
                        Weight = data.Weight,
                        Images = data.KoiImages.Select(x => x.ImageUrl).ToList()
                    };

                    return new ServiceResult<KoiFishDetailViewModel>(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
                else
                {
                    return new ServiceResult<KoiFishDetailViewModel>(Common.Constant.StatusCode.FailedStatusCode, "This koiFish does not exist.");
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<KoiFishDetailViewModel>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        private async Task ValidateKoiFishAsync(UpdateKoiFishRequestModel request)
        {
            var result = await _unitOfWork.KoiFishRepository.IsKoiInAuction(request.Id);
            if (result)
            {
                throw new InvalidOperationException("This koiFish is in an auction session and cannot be updated.");
            }
            if (request.StartingPrice <= 0)
            {
                throw new InvalidOperationException("The starting price must be greater than 0.");
            }

            if (request.CurrentPrice <= 0)
            {
                throw new InvalidOperationException("The current price must be greater than 0.");
            }

            if (request.Age <= 0)
            {
                throw new InvalidOperationException("The age must be greater than 0.");
            }
        }

        public async Task<ServiceResult<int>> UpdateKoiFishAsync(UpdateKoiFishRequestModel request)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetKoiFishByIdAsync(request.Id);

                if (koiFish == null)
                {
                    return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, "This koiFish does not exist.");
                }

                await ValidateKoiFishAsync(request);

                koiFish.Description = request.Description;
                koiFish.Name = request.Name;
                koiFish.StartingPrice = request.StartingPrice;
                koiFish.CurrentPrice = request.CurrentPrice;
                koiFish.Age = request.Age;
                koiFish.Origin = request.Origin;
                koiFish.Weight = request.Weight;
                koiFish.Length = request.Length;
                koiFish.ColorPattern = request.ColorPattern;

                _unitOfWork.KoiFishRepository.Update(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult<int>(Common.Constant.StatusCode.SuccessStatusCode, "Update koiFish successfully.", koiFish.Id);
            }
            catch (InvalidOperationException e)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
            catch (Exception e)
            {
                return new ServiceResult<int>(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
