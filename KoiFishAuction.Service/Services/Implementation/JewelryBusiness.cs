using JewelryAuction.Business.Business.Interface;
using JewelryAuction.Business.RequestModels.Jewelry;
using JewelryAuction.Business.ViewModels;
using JewelryAuction.Data;
using JewelryAuction.Data.Models;

namespace JewelryAuction.Business.Business.Implementation
{
    public class JewelryBusiness : IJewelryBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseStorageBusiness _firebaseStorageBusiness;

        public JewelryBusiness(UnitOfWork unitOfWork, IFirebaseStorageBusiness firebaseStorageBusiness)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageBusiness = firebaseStorageBusiness;
        }

        public async Task<JewelryAuctionResult> CanUpdateJewelryAsync(int id)
        {
            try
            {
                var jewelry = await _unitOfWork.JewelryRepository.GetJewelryByIdAsync(id);
                if (jewelry == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This jewelry does not exist.");
                }

                var result = await _unitOfWork.JewelryRepository.IsJewelryInAuction(id);
                if (result)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "You cannot change the information for this jewelry because it is in auction.");
                }

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> CreateJewelryAsync(CreateJewelryRequestModel request, int sellerid)
        {
            try
            {
                if (request.StartingPrice <= 0)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The starting price must be greater than 0.");
                }

                var jewelry = new Jewelry()
                {
                    ImageUrl = await _firebaseStorageBusiness.UploadJewelryImage(request.Image),
                    StartingPrice = request.StartingPrice,
                    CurrentPrice = request.StartingPrice,
                    Description = request.Description,
                    Name = request.Name,
                    SellerId = sellerid
                };

                await _unitOfWork.JewelryRepository.CreateAsync(jewelry);
                await _unitOfWork.JewelryRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Create successful jewelry.", jewelry);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> DeleteJewelryAsync(int id)
        {
            try
            {
                var jewelry = await _unitOfWork.JewelryRepository.GetJewelryByIdAsync(id);
                if (jewelry == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This jewelry does not exist.");
                }

                await _unitOfWork.JewelryRepository.RemoveAsync(jewelry);
                await _unitOfWork.JewelryRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Delete jewelry successful.");
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> GetAllJewelriesAsync(int sellerid)
        {
            try
            {
                var data = await _unitOfWork.JewelryRepository.GetAllJewelriesAsync(sellerid);
                if (data.Count == 0)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "You do not currently own any jewelry.");
                }
                else
                {
                    var result = data.Select(jewelry => new JewelryViewModel()
                    {
                        JewelryId = jewelry.JewelryId,
                        CurrentPrice = jewelry.CurrentPrice,
                        Description = jewelry.Description,
                        ImageUrl = jewelry.ImageUrl,
                        Name = jewelry.Name,
                        SellerUsername = jewelry.Seller.Username,
                        StartingPrice = jewelry.StartingPrice,
                    }).ToList();

                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> GetJewelryByIdAsync(int id)
        {
            try
            {
                var data = await _unitOfWork.JewelryRepository.GetJewelryByIdAsync(id);
                if (data != null)
                {
                    var result = new JewelryViewModel()
                    {
                        CurrentPrice = data.CurrentPrice,
                        Description = data.Description,
                        ImageUrl = data.ImageUrl,
                        JewelryId = data.JewelryId,
                        Name = data.Name,
                        SellerUsername = data.Seller.Username,
                        StartingPrice = data.StartingPrice
                    };
                    return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, result);
                }
                else
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This jewelry does not exist.");
                }
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> UpdateJewelryAsync(UpdateJewelryRequestModel request, int sellerId)
        {
            try
            {
                var jewelry = await _unitOfWork.JewelryRepository.GetJewelryByIdAsync(request.JewelryId);

                if (jewelry == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This jewelry does not exist.");
                }

                if (request.StartingPrice <= 0)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "The starting price must be greater than 0.");
                }

                jewelry.Description = request.Description;
                jewelry.Name = request.Name;
                jewelry.StartingPrice = request.StartingPrice;
                jewelry.SellerId = sellerId;

                _unitOfWork.JewelryRepository.Update(jewelry);
                await _unitOfWork.JewelryRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode, "Update jewelry successful.");
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }

        public async Task<JewelryAuctionResult> UpdateJewelryPriceAsync(int jewelryid, decimal price)
        {
            try
            {
                var jewelry = await _unitOfWork.JewelryRepository.GetJewelryByIdAsync(jewelryid);

                if (jewelry == null)
                {
                    return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, "This jewelry does not exist.");
                }

                jewelry.CurrentPrice = price;

                _unitOfWork.JewelryRepository.Update(jewelry);
                await _unitOfWork.JewelryRepository.SaveAsync();

                return new JewelryAuctionResult(Common.Constant.StatusCode.SuccessStatusCode);
            }
            catch (Exception e)
            {
                return new JewelryAuctionResult(Common.Constant.StatusCode.FailedStatusCode, e.Message);
            }
        }
    }
}
