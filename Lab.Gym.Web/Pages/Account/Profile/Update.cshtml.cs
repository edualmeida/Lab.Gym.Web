// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using AutoMapper;
using Lab.Gym.Web.Application.Models;
using Lab.Gym.Web.Application.Services;
using Lab.Gym.Web.Pages.Shared.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace Lab.Gym.Web.Pages.Manage.Profile
{
    [Authorize]
    public class UpdateModel(
        IProfileService profileService,
        IMapper mapper) : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }
        
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public SelectList Roles { get; set; }
        public bool IsEdit { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            //Input = InputModel.GetFromClaims((ClaimsIdentity)User.Identity, "");
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var userId = claims.GetValue("sub", "");
            var profile = await profileService.GetProfile(claims.GetValue("sub", ""));

            Input = mapper.Map<UserProfile, InputModel>(profile);
            Input.Email = claims.GetValue("email", "");
            Input.UserId = userId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await profileService.UpdateProfile(Input.UserId, mapper.Map<InputModel, UserProfile>(Input));
            StatusMessage = $" Profile saved successfully";

            return Redirect("/Account/Profile/Index");
        }
    }
}
