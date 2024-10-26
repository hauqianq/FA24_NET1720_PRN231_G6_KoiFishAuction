﻿using KoiFishAuction.Common.RequestModels.KoiFish;
using KoiFishAuction.Common.ViewModels.KoiFish;
using KoiFishAuction.Service.Services;

namespace KoiFishAuction.MVC.Services.Interfaces
{
    public interface IKoiFishApiClient
    {
        Task<ServiceResult<List<KoiFishViewModel>>> GetAllKoiFishesAsync();
        Task<ServiceResult<KoiFishViewModel>> GetKoiFishByIdAsync(int id);
        Task<ServiceResult<int>> CreateKoiFishAsync(CreateKoiFishRequestModel request);
        Task<ServiceResult<bool>> DeleteKoiFishAsync(int id);
        Task<ServiceResult<int>> UpdateKoiFishAsync(int id, UpdateKoiFishRequestModel request);
        Task<ServiceResult<int>> UpdateKoiFishPriceAsync(int id, decimal price);
    }
}
