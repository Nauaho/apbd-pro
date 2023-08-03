using Funds.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using WebApi.Models.DTOs;

namespace Funds.Data
{
    public interface IStocksService
    {
        Task<IEnumerable<Stock>> GetWatchlistAsync(string login, string token);
        Task<IEnumerable<StocksPreview>> SearchAsync(string? input, string token);
        Task Subscribe(string login, string symbol, string token);
        Task Unsubscribe(string login, string symbol, string token);
    }
    public class StocksService : IStocksService
    {
        private readonly string _apiLink;
        private readonly IHttpClientFactory _httpClientFactory;
        public StocksService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiLink = configuration["WebApi:Default"]
                            ?? throw new NullReferenceException();
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<Stock>> GetWatchlistAsync(string login, string token)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await client.GetAsync(_apiLink + $"/api/users/{login}/subs");
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<IEnumerable<Stock>>(await response.Content.ReadAsStringAsync());
            return result;
        }
        public async Task Unsubscribe(string login, string symbol, string token)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await client.DeleteAsync(_apiLink + $"/api/users/{login}/subs/unsubscribe?ticker={symbol}");
            response.EnsureSuccessStatusCode();
        }

        public async Task Subscribe(string login, string symbol, string token)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await client.DeleteAsync(_apiLink + $"/api/users/{login}/subs/subscribe?ticker={symbol}");
        }

        public async Task<IEnumerable<StocksPreview>> SearchAsync(string? input,string token)
        {
            if (input == null)
                return Enumerable.Empty<StocksPreview>();
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await client.GetAsync(_apiLink + $"/api/stocks/search?input={input}");
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<StocksPreview>();
            var result = JsonConvert.DeserializeObject<IEnumerable<StocksPreview>>(await response.Content.ReadAsStringAsync());
            return result;
        }
    }
}
