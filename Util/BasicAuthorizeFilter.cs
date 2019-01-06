using System;
using System.Text;
using System.Linq;
using stacsnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;

namespace stacsnet.Util {

    public class BasicAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string realm;
        public BasicAuthorizeFilter(string realm = null)
        {
            this.realm = realm;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];
                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    return;
                }
            }
            // Return authentication type (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic";
            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.HttpContext.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }
            // Return unauthorized
            context.Result = new UnauthorizedResult();
        }

        public bool IsAuthorized(string username, string password)
        {
            return true;
            using (var acontext = new SnContext()) {
                var hasher = new PasswordHasher<Account>();
                var account = acontext.Accounts.FirstOrDefault( a => (a.uname == username && (hasher.VerifyHashedPassword(a, a.pwhash, password) == PasswordVerificationResult.Success) && a.verified));
                if (account != null && account.verified == true)
                    return true;
                else 
                    return false;
            }
        }
    }
}