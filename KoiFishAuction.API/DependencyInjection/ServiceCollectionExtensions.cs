using JewelryAuction.Business.Business.Implementation;
using KoiFishAuction.Data;
using KoiFishAuction.Data.Models;
using KoiFishAuction.Data.Repository;
using KoiFishAuction.Service.Services.Implementation;
using KoiFishAuction.Service.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace KoiFishAuction.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationServicesConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtConfiguration:Issuer"],
                    ValidAudience = configuration["JwtConfiguration:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfiguration:Key"]))

                };
            });
            return services;
        }
        public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization using the Bearer scheme. \"Bearer\" is not needed.Just paste the jwt"
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            return services;
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddScoped<UnitOfWork>();

            services.AddScoped<UserRepository>();
            services.AddScoped<BidRepository>();
            services.AddScoped<AuctionSessionRepository>();
            services.AddScoped<KoiFishRepository>();
            services.AddScoped<KoiImageRepository>();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<IAuctionSessionService, AuctionSessionService>();
            services.AddScoped<IKoiFishService, KoiFishService>();
            services.AddScoped<IKoiImageService, KoiImageService>();


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
