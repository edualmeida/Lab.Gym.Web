using Lab.Gym.Web.Application.Models;
using Refit;

namespace Lab.Gym.Web.Application.Clients
{
    [Headers("Authorization: Bearer")]
    public interface IProfileApiClient
    {
        [Get("/UserProfile/{userId}")]
        Task<UserProfile> GetProfile(string userId);

        [Post("/UserProfile/{userId}")]
        Task UpdateProfile(string userId, UserProfile userProfile);
    }
}
