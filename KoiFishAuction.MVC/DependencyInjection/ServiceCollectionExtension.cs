using KoiFishAuction.MVC.Services.Implements;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KoiFishAuction.MVC.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceConfigure(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/Index";

                options.AccessDeniedPath = "/User/Forbidden/";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddScoped<IUserApiClient, UserApiClient>();
            services.AddScoped<IKoiFishApiClient, KoiFishApiClient>();
            services.AddScoped<IBidApiClient, BidApiClient>();
            services.AddScoped<IAuctionSessionApiClient, AuctionSessionApiClient>();

            return services;
        }
    }
}
