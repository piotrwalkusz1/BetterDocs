using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using BetterDocs.Data;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BetterDocs.Filters
{
    public class BasicAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
                

            }

            var headers = context.HttpContext.Request.Headers["Authorization"];
            if (headers.IsNullOrEmpty() || !headers[0].StartsWith("Basic "))
            {
                return;   
            }

            var token = headers[0].Substring(6);
            var decryptedToken = Convert.FromBase64String(token);
            var credentials = Encoding.UTF8.GetString(decryptedToken).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            var dbContext = (ApplicationDbContext) context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
            var user = dbContext.ApplicationUsers.FirstOrDefault(u => u.Email.Equals(username));

            if (user == null)
            {
                return;
            }
            
            var identity = new GenericIdentity(username);
            identity.AddClaim(new Claim(new ClaimsIdentityOptions().UserIdClaimType, user.Id));
            var principal = new ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }
    }
}