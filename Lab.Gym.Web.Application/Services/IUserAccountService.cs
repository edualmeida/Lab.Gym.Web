using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Services
{
    public interface IUserAccountService
    {
        Task ChangePassword(string userId, ChangePassword changePassword);
    }
}
