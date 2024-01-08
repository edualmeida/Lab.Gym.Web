using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab.Core.IdentityServer.Pages.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    [BindProperty] 
    public string LogoutId { get; set; }

    public Index()
    {
    }

    public async Task<IActionResult> OnGet(string logoutId)
    {
        return SignOut("Cookies", "oidc");
    }
}