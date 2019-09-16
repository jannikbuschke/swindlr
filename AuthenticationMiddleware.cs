using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwindlR
{


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
