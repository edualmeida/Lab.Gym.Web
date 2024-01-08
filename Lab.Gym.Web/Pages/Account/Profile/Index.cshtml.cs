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
using Lab.Gym.Web.Pages.Shared.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace Lab.Core.IdentityServer.Pages.Manage.Profile
{
    [Authorize]
    public class ProfileModel : PageModel
    {

        public ProfileModel()
        {
        }

        [TempData]
        public string StatusMessage { get; set; }
        
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public SelectList Roles { get; set; }
        public bool IsEdit { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, string userId = null)
        {
            Input = InputModel.GetFromClaims((ClaimsIdentity)User.Identity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            return Redirect("./Profile/Update");
        }
    }
}
