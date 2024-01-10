using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Services
{
    public interface IUserAccountService
    {
        Task<HttpCallResult> ChangePassword(string userId, ChangePassword changePassword);
    }
}
