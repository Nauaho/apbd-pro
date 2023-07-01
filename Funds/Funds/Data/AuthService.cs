﻿using Funds.Models;
using Funds.Models.DTO;
using System.Text;
using Newtonsoft.Json;

namespace Funds.Data
{

    public interface IAuthService
    {
        Task<string?> LoginUser(string username, string password);
        Task<TokenSet?> RegisterUser(string username, string password, string? email);
    }
    public class AuthService : IAuthService
    {
        private readonly string _apiLink;

        public AuthService(IConfiguration configuration)
        {
            _apiLink = configuration["WebApi:Default"]
                            ?? throw new NullReferenceException();
        }
        public async Task<string?> LoginUser(string username, string password)
        {

            var json = JsonConvert.SerializeObject(new
                {
                    Email = (string?)null,
                    Login = username,
                    Password = password
                });
            using HttpClient client = new();
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_apiLink+ "/api/users/login", content);
            if (!response.IsSuccessStatusCode) 
                return null;
            var value = JsonConvert.DeserializeObject<TokenDTO>(await response.Content.ReadAsStringAsync() );
            if (value is null)
                return null;
            return value.Token;
        }

        public async Task<TokenSet?> RegisterUser(string username, string password, string? email)
        {
            var json = JsonConvert.SerializeObject(new
            {
                Email = email,
                Login = username,
                Password = password
            });
            using HttpClient client = new();
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_apiLink + "/api/users/register", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = JsonConvert.DeserializeObject<TokenSet>( await response.Content.ReadAsStringAsync() );
            return result;
        }
    }
}