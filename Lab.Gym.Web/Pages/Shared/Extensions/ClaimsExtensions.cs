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
    }
}
