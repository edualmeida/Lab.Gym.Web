using Lab.Gym.Web.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Gym.Web.Application.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetProfile(string userId);
        Task UpdateProfile(string userId, UserProfile userProfile);
    }
}
