using System.Net.Mail;
using System.Security.Claims;
using Lab.Gym.Web.Pages.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Lab.Gym.Web.Pages.Shared.Components.RightNavbarUserArea
{
    public class RightNavbarUserAreaViewComponent : ViewComponent
    {
        public RightNavbarUserAreaViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var model = new RightNavbarUserAreaViewModel
            {
                //LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                Username = GetUsername(claimsIdentity?.Claims?.GetValue("email", "")),
                IsAuthenticated = User?.Identity?.IsAuthenticated ?? false
            };

            return View(model);
        }

        private static string GetUsername(string email)
        {
            if(string.IsNullOrEmpty(email))
                return string.Empty;

            return new MailAddress(email).User;
        }
    }
}

