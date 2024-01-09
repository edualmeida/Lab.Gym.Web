using Lab.Gym.Web.Application.Exceptions;
using Lab.Gym.Web.Application.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Lab.Gym.Web.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly HttpClient _httpClient;

        public UserAccountService(HttpClient httpClient) 
        {
            _httpClient = httpClient;

        }

        public async Task ChangePassword(string userId, ChangePassword changePassword)
        {
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<ChangePassword>($"UserAccount/{userId}", changePassword);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<RequestError>(contentStream);

                throw new RequestException(result.Errors);
            }
        }
    }
}
