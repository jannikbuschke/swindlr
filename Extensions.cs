using Microsoft.AspNetCore.Builder;
using System.Net.Http;

namespace SwindlR
{
    public static class Extensions
    {
        public static void UseSwindlRAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticationMiddleware>();
        }

        public static void Swindle(this HttpClient client, string userMame, string userId)
        {
            client.DefaultRequestHeaders.Add(AuthenticationMiddleware.UsernameHeader, userMame);
            client.DefaultRequestHeaders.Add(AuthenticationMiddleware.UserIdHeader, userId);
        }
    }
}
