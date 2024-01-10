// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Lab.Gym.Web.Application.Services;
using Lab.Gym.Web.Pages.Shared.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab.Gym.Web.Pages.Account.ChangePassword
{
    [Authorize]
    public class ChangePasswordModel : UserPageModel
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

            var callResult = await _userAccountService.ChangePassword(UserId, new Application.Models.ChangePassword()
            {
                NewPassword = Input.NewPassword,
                OldPassword = Input.OldPassword,
            });

            if (!callResult.Succeeded)
            {
                StatusMessage = callResult.ErrorMessage;
            }
            else
            {
                StatusMessage = "Your password has been changed.";
            }

            return Page();
        }
    }
}
