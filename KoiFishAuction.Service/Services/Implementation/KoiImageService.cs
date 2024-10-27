using KoiFishAuction.Data.Models;
using KoiFishAuction.Data;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Http;

public class KoiImageService : IKoiImageService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IFirebaseStorageService _firebaseStorageService;

    public KoiImageService(UnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService)
    {
        _unitOfWork = unitOfWork;
        _firebaseStorageService = firebaseStorageService;
    }

}
