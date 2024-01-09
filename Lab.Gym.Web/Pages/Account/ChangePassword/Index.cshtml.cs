// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab.Gym.Web.Application.Exceptions;
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

        [BindProperty]
        public InputModel Input { get; set; }

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

            try
            {
                await _userAccountService.ChangePassword(userId, new Application.Models.ChangePassword()
                {
                    NewPassword = Input.NewPassword,
                    OldPassword = Input.OldPassword,
                });
            }
            catch (RequestException error)
            {
                StatusMessage = error.Errors.FirstOrDefault().Description;
                return Page();
            }
            catch (Exception)
            {
                throw;
            }

            StatusMessage = "Your password has been changed.";

            return SignOut("Cookies", "oidc");
        }
    }
}
