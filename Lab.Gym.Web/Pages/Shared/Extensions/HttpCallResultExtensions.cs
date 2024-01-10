using Lab.Gym.Web.Application.Models;
using System.Text;

namespace Lab.Gym.Web.Pages.Shared.Extensions
{
    public static class HttpCallResultExtensions
    {
        public static string AggregateErrors(this HttpCallResult httpCallResult)
        {
            if (httpCallResult.Succeeded)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var error in httpCallResult.Errors)
            {
                builder.AppendLine(error.Description);
            }

            return builder.ToString();
        }
    }
}
