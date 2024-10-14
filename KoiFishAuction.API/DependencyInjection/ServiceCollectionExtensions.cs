using JewelryAuction.Business.Business.Implementation;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Data.Repository;
using KoiFishAuction.Service.Services.Implementation;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace KoiFishAuction.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddScoped<UnitOfWork>();

            services.AddScoped<UserRepository>();
            services.AddScoped<BidRepository>();
            services.AddScoped<AuctionSessionRepository>();
            services.AddScoped<KoiFishRepository>();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<IAuctionSessionService, AuctionSessionService>();
            services.AddScoped<IKoiFishService, KoiFishService>();


            return services;
        }

        public static IServiceCollection AddSqlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FA24_NET1720_PRN231_G6_KoiFishAuctionContext>(options =>
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
