// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Lab.Gym.Web.Application.Services;
using Lab.Gym.Web.Pages.Shared.Components;
using Lab.Gym.Web.Pages.Shared.Extensions;
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

            StatusMessage = callResult.Succeeded ? "Your password has been changed." : callResult.AggregateErrors();

            return Page();
        }
    }
}
