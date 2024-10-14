using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Service.Services.Interface;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class KoiFishService : IKoiFishService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageBusiness;

        public KoiFishService(UnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageBusiness)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageBusiness = firebaseStorageBusiness;
        }

        public async Task<ServiceResult> CanUpdateKoiFishAsync(int id)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
                if (koiFish == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This koi fish does not exist.");
                }

                var result = await _unitOfWork.KoiFishRepository.IsKoiInAuction(id);
                if (result)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "You cannot change the information for this koiFish because it is in auction.");
                }

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> CreateKoiFishAsync(CreateKoiFishRequestModel request, int sellerid)
        {
            try
            {
                if (request.StartingPrice <= 0)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "The starting price must be greater than 0.");
                }

                var koiFish = new KoiFish()
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartingPrice = request.StartingPrice,
                    CurrentPrice = request.CurrentPrice,
                    ImageUrl = await _firebaseStorageBusiness.UploadKoiFishImage(request.ImageUrl),
                    Age = request.Age,
                    Origin = request.Origin,
                    Weight = request.Weight,
                    Length = request.Length,
                    ColorPattern = request.ColorPattern,
                    SellerId = sellerid,
                };

                await _unitOfWork.KoiFishRepository.CreateAsync(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, "Create successful koi fish.", koiFish);
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> DeleteKoiFishAsync(int id)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
                if (koiFish == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This koi fish does not exist.");
                }

                await _unitOfWork.KoiFishRepository.RemoveAsync(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, "Delete koi fish successful.");
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> GetAllKoiFishesAsync(int sellerid)
        {
            try
            {
                var data = await _unitOfWork.KoiFishRepository.GetAllKoiFishesAsync(sellerid);
                if (data.Count == 0)
                {
                    return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, "You do not currently own any koiFish.");
                }
                else
                {
                    var result = data.Select(KoiFish => new KoiFishViewModel()
                    {
                        Age = KoiFish.Age,
                        ColorPattern = KoiFish.ColorPattern,
                        CurrentPrice = KoiFish.CurrentPrice,
                        Description = KoiFish.Description,
                        Id = KoiFish.Id,
                        ImageUrl = KoiFish.ImageUrl,
                        Length = KoiFish.Length,
                        Name = KoiFish.Name,
                        Origin = KoiFish.Origin,
                        SellerUserName = KoiFish.Seller.Username,
                        StartingPrice = KoiFish.StartingPrice,
                        Weight = KoiFish.Weight
                    }).ToList();

                    return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> GetKoiFishByIdAsync(int id)
        {
            try
            {
                var data = await _unitOfWork.KoiFishRepository.GetKoiFishByIdAsync(id);
                if (data != null)
                {

                    var result = new KoiFishViewModel()
                    {
                        Age = data.Age,
                        ColorPattern = data.ColorPattern,
                        CurrentPrice = data.CurrentPrice,
                        Description = data.Description,
                        Id = data.Id,
                        ImageUrl = data.ImageUrl,
                        Length = data.Length,
                        Name = data.Name,
                        Origin = data.Origin,
                        SellerUserName = data.Seller.Username,
                        StartingPrice = data.StartingPrice,
                        Weight = data.Weight
                    };  

                    return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
                else
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This koiFish does not exist.");
                }
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> UpdateKoiFishAsync(int id, UpdateKoiFishRequestModel request)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetKoiFishByIdAsync(id);

                if (koiFish == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This koiFish does not exist.");
                }

                if (request.StartingPrice <= 0)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "The starting price must be greater than 0.");
                }

                koiFish.Description = request.Description;
                koiFish.Name = request.Name;
                koiFish.StartingPrice = request.StartingPrice;

                _unitOfWork.KoiFishRepository.Update(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode, "Update koiFish successful.");
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<ServiceResult> UpdateKoiFishPriceAsync(int koiFishid, decimal price)
        {
            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetKoiFishByIdAsync(koiFishid);

                if (koiFish == null)
                {
                    return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, "This koiFish does not exist.");
                }

                koiFish.CurrentPrice = price;

                _unitOfWork.KoiFishRepository.Update(koiFish);
                await _unitOfWork.KoiFishRepository.SaveAsync();

                return new ServiceResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new ServiceResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
