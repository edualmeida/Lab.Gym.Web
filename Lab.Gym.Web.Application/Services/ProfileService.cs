using Lab.Gym.Web.Application.Clients;
using Lab.Gym.Web.Application.Exceptions;
using Lab.Gym.Web.Application.Models;
using System.Net.Http;
using System.Text.Json;
using Lab.Gym.Web.Application.Extensions;

namespace Lab.Gym.Web.Application.Services
{
    public class ProfileService(HttpClient httpClient) : IProfileService
    {
        public Task UpdateProfile(string userId, UserProfile userProfile)
        {
            return httpClient.PostAsync($"UserProfile/{userId}", userProfile);
        }

        public async Task<UserProfile> GetProfile(string userId)
        {
            var result = await httpClient.GetAsync<UserProfile>($"UserProfile/{userId}");

            return result;
        }
    }
}
