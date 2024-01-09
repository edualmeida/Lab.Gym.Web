using Lab.Gym.Web.Application.Clients;
using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Services
{
    public class ProfileService : IProfileService
    {
        //private readonly IProfileApiClient _profileApiClient;

        public ProfileService() 
        {
            //_profileApiClient = profileApiClient;
        }

        public Task UpdateProfile(string userId, UserProfile userProfile)
        {
            //return _profileApiClient.UpdateProfile(userId, userProfile);
            return Task.CompletedTask;
        }

        public Task<UserProfile> GetProfile(string userId)
        {
            //return _profileApiClient.GetProfile(userId);
            return Task.FromResult<UserProfile>(null);
        }
    }
}
