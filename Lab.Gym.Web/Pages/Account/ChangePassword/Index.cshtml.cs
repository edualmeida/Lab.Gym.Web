// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab.Gym.Web.Application.Services;
using Lab.Gym.Web.Pages.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Lab.Gym.Web.Pages.Account.ChangePassword
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IUserAccountService _userAccountService;

        public ChangePasswordModel(
            ILogger<ChangePasswordModel> logger,
            IUserAccountService userAccountService)
        {
            _logger = logger;
            _userAccountService = userAccountService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var userId = claims.GetValue("sub", "");
            
            await _userAccountService.ChangePassword(userId, new Application.Models.ChangePassword()
            {
                NewPassword = Input.NewPassword,
                OldPassword = Input.OldPassword,
            });
            StatusMessage = "Your password has been changed.";
            SignOut("Cookies");
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            //if (!changePasswordResult.Succeeded)
            //{
            //    foreach (var error in changePasswordResult.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //    return Page();
            //}

            //await _signInManager.RefreshSignInAsync(user);
            //_logger.LogInformation("User changed their password successfully.");
            //

            return RedirectToPage();
        }
    }
}
