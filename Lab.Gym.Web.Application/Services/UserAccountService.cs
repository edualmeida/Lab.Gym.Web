using Lab.Gym.Web.Application.Exceptions;
using Lab.Gym.Web.Application.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Lab.Gym.Web.Application.Extensions;

namespace Lab.Gym.Web.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly HttpClient _httpClient;

        public UserAccountService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<HttpCallResult> ChangePassword(string userId, ChangePassword changePassword)
        {
            return await _httpClient.HandlePostAsync($"UserAccount/changepassword/{userId}", changePassword);
        }
    }
}
