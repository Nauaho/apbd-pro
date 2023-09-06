using Funds.Models;
using System.Text;
using Newtonsoft.Json;
using WebApi.Models.DTOs;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Funds.Data
{
    public interface IAuthService
    {
        public string Action { get; set; }
        public bool IsLoggedIn { get; }
        public string Login { get; }
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public Task<TokenSet?> LoginUser(string username, string password);
        public Task<bool> LogOut();
        public Task<TokenSet?> RefreshTheToken();
        public Task<TokenSet?> RefreshTheToken(string username, string refreshToken);
        public Task<TokenSet?> RegisterUser(string username, string password, string? email);
    }
    public class AuthService : IAuthService
    {
        public string Action { get; set; } = "";
        public bool IsLoggedIn { get { return !(RefreshToken == null || Login == null); } }
        public string Login { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;

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
            try
            {
                var json = JsonConvert.SerializeObject(new
                {
                    Email = (string?)null,
                    Login = username,
                    Password = password
                });
                using HttpClient client = _httpClientFactory.CreateClient();
                using StringContent content = new(json, Encoding.UTF8, "application/json");
                using var response = await client.PostAsync(_apiLink + "/api/users/login", content);
                if (!response.IsSuccessStatusCode)
                    return null;
                var value = JsonConvert.DeserializeObject<TokenSet>(await response.Content.ReadAsStringAsync());
                if (value is null)
                    return null;
                Login = username;
                AccessToken = value.AccessToken;
                RefreshToken = value.RefreshToken;
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TokenSet?> RegisterUser(string username, string password, string? email)
        {
            try 
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
                if(result is null)
                    return null;
                Login = username;
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                return result;
            }
            catch (Exception)
            {
                return null;
            }
}
        public async Task<TokenSet?> RefreshTheToken(string username, string refreshToken)
        {
            try
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
                if (result is null)
                    return null;
                Login = username;
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                Console.WriteLine("Access Token has been Updated");
                return result;
            }
            catch (Exception) 
            {
                Login = null!;
                AccessToken = null!;
                RefreshToken = null!;
                return null;
            }
        }

        public async Task<bool> LogOut()
        {
            try
            {

                using HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                using var response = await client.DeleteAsync(_apiLink + $"/api/users/logout?token={RefreshToken}");
                AccessToken = null!;
                RefreshToken = null!;
                Login = null!;
                return true;
            }
            catch (Exception) 
            {
                AccessToken = null!;
                RefreshToken = null!;
                Login = null!;
                return false;
            }
        }

        public Task<TokenSet?> RefreshTheToken()
        {
            return RefreshTheToken(Login, RefreshToken);
        }
    }
}
