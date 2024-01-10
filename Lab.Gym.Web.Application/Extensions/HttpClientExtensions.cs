using Lab.Gym.Web.Application.Exceptions;
using Lab.Gym.Web.Application.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Lab.Gym.Web.Application.Extensions
{
    public static class HttpClientExtensions
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public static async Task<HttpCallResult> HandlePostAsync<TValue>(this HttpClient httpClient, string? requestUri, TValue value)
        {
            try
            {
                await httpClient.PostAsync(requestUri, value);
            }
            catch (BadRequestException exception)
            {
                return new HttpCallResult(exception.Errors.FirstOrDefault().Description);
            }
            catch (Exception)
            {
                throw;
            }

            return new SuccessHttpCallResult();
        }

        public static async Task<HttpCallResult> HandlePutAsync<TValue>(this HttpClient httpClient, string? requestUri, TValue value)
        {
            try
            {
                await httpClient.PutAsync(requestUri, value);
            }
            catch (BadRequestException exception)
            {
                return new HttpCallResult(exception.Message);
            }
            catch (Exception)
            {
                throw;
            }

            return new SuccessHttpCallResult();
        }

        public static async Task PostAsync<TValue>(this HttpClient httpClient, string? requestUri, TValue value)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(requestUri, value);

            await HandleBadRequest(httpResponseMessage);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public static async Task PutAsync<TValue>(this HttpClient httpClient, string? requestUri, TValue value)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync<TValue>(requestUri, value);

            await HandleBadRequest(httpResponseMessage);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public static async Task<TResult> GetAsync<TResult>(this HttpClient httpClient, string? requestUri)
        {
            var httpResponseMessage = await httpClient.GetAsync(requestUri);

            httpResponseMessage.EnsureSuccessStatusCode();

            var result = await DeserializeResult<TResult>(httpResponseMessage);

            return result;
        }

        private static async Task HandleBadRequest(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var deserializedError = await DeserializeResult<RequestError>(httpResponseMessage);

                throw new BadRequestException(deserializedError.Errors);
            }
        }

        private static async Task<TResult> DeserializeResult<TResult>(HttpResponseMessage httpResponseMessage)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<TResult>(contentStream, _options);

            if (result == null)
            {
                throw new DeserializationResultException($"Error when deserializing error Length: '{contentStream.Length}'");
            }

            return result;
        }
    }
}
