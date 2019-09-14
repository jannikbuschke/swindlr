using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

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


    public class AuthenticationMiddleware
    {
        public const string AuthenticationTypeName = "SwindlRAuthentication";

        public const string UsernameHeader = "x-swindlr-username";
        public const string UserIdHeader = "x-swindlr-userid";

        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(UsernameHeader))
            {
                var name = context.Request.Headers[UsernameHeader].First();
                var id = context.Request.Headers.Keys.Contains(UserIdHeader)
                        ? context.Request.Headers[UserIdHeader].First() : "";
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, name),
                        new Claim(ClaimTypes.NameIdentifier, id),
                    }, AuthenticationTypeName);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.User = claimsPrincipal;
            }

            await _next(context);
        }
    }
}
