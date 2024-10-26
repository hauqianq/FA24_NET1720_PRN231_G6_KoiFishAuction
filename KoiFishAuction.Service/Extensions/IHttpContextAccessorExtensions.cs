using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KoiFishAuction.Service.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetCurrentUserName(this IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
        }

        public static string GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        }
    }
}
