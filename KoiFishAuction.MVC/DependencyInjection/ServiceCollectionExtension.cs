using KoiFishAuction.MVC.Services.Implements;
using KoiFishAuction.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

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

            services.AddHttpClient<NotificationApiClient>(static (sp, client) => {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                
                client.BaseAddress = new Uri(Common.Constant.EndPoint.APIEndPoint);

                var session = httpContextAccessor.HttpContext?.Session.GetString("Token");
                if (!string.IsNullOrEmpty(session)) {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                return new SocketsHttpHandler {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                };
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            return services;
        }
    }
}
