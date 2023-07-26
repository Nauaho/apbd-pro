using Funds.Models;
using System.Text;
using Newtonsoft.Json;
using WebApi.Models.DTOs;

namespace Funds.Data
{
    public interface IAuthService
    {
        Task<TokenSet?> LoginUser(string username, string password);
        Task<TokenSet?> RefreshToken(string refreshToken);
        Task<TokenSet?> RegisterUser(string username, string password, string? email);
    }
    public class AuthService : IAuthService
    {
        private readonly string _apiLink;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiLink = configuration["WebApi:Default"]
                            ?? throw new NullReferenceException();
            _httpClientFactory = httpClientFactory;
        }


        public async Task<TokenSet?> LoginUser(string username, string password)
        {

            var json = JsonConvert.SerializeObject(new
                {
                    Email = (string?)null,
                    Login = username,
                    Password = password
                });
            using HttpClient client = _httpClientFactory.CreateClient();
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(_apiLink+ "/api/users/login", content);
            if (!response.IsSuccessStatusCode) 
                return null;
            var value = JsonConvert.DeserializeObject<TokenSet>(await response.Content.ReadAsStringAsync() );
            Console.WriteLine(value);
            if (value is null)
                return null;
            return value;
        }

        public async Task<TokenSet?> RegisterUser(string username, string password, string? email)
        {
            var json = JsonConvert.SerializeObject(new
            {
                Email = email,
                Login = username,
                Password = password
            });
            using HttpClient client = _httpClientFactory.CreateClient();
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(_apiLink + "/api/users/register", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = JsonConvert.DeserializeObject<TokenSet>( await response.Content.ReadAsStringAsync() );
            return result;
        }
        public async Task<TokenSet?> RefreshToken(string refreshToken)
        {
            var json = JsonConvert.SerializeObject(new
            {
                rToken = refreshToken
            });
            using HttpClient client = _httpClientFactory.CreateClient();
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(_apiLink + "/api/users/refresh/token", content);
            if (!response.IsSuccessStatusCode)
                return null;
            var result = JsonConvert.DeserializeObject<TokenSet>(await response.Content.ReadAsStringAsync());
            return result;
        }
    }
}
