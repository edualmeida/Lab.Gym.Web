using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab.Core.IdentityServer.Pages.Login;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    [BindProperty] 
    public string LogoutId { get; set; }

    public Index()
    {
    }

    public async Task<IActionResult> OnGet(string logoutId)
    {
        return Redirect("./Profile/Index");
    }
}