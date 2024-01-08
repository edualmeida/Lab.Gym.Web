using Lab.Gym.Web.Application.Models;
using Refit;

namespace Lab.Gym.Web.Application.Clients
{
    [Headers("Authorization: Bearer")]
    public interface IUserAccountApiClient
    {
        [Post("/UserAccount/{userId}")]
        Task ChangePassword(string userId, ChangePassword changePassword);
    }
}
