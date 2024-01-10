using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Lab.Gym.Web.Pages.Shared.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetValue(this IEnumerable<Claim> claims, string claimType, string defaultValue = "")
        {
            var val = claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }

        public static string GetUserId(this PageModel page)
        {
            var claims = ((ClaimsIdentity)page.User.Identity).Claims;
            var userId = claims.GetValue("sub", "");
            return userId;
        }
    }
}
