using Lab.Gym.Web.Application.Clients;
using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountApiClient _userAccountApiClient;

        public UserAccountService(IUserAccountApiClient userAccountApiClient) 
        {
            _userAccountApiClient = userAccountApiClient;
        }

        public Task ChangePassword(string userId, ChangePassword changePassword)
        {
            return _userAccountApiClient.ChangePassword(userId, changePassword);
        }
    }
}
