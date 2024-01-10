﻿using Lab.Gym.Web.Pages.Shared.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Lab.Gym.Web.Pages.Shared.Components
{
    public class UserPageModel : PageModel
    {
        private ClaimsIdentity identity = null;
        public string UserId { get { return GetUserId(); } }

        private ClaimsIdentity GetClaimsIdentity()
        {
            if(identity == null)
            {
                if (User == null)
                {
                    throw new Exception("User is null");
                }

                if (User.Identity == null)
                {
                    throw new Exception("User.Identity is null");
                }

                identity = User.Identity as ClaimsIdentity;

                if(identity == null)
                {
                    throw new Exception("ClaimsIdentity is null");
                }
            }

            return identity;
        }

        private IEnumerable<Claim> GetClaims()
        {
            return GetClaimsIdentity().Claims;
        }

        private string GetUserId()
        {
            return GetClaims().GetValue("sub", "");
        }
    }
}
