using Funds.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Funds.Data
{
    public interface IStocksService
    {
        public Task<IEnumerable<StocksPreview>> SearchAsync(string? input);
        public Task<Stock?> GetStockInfoAsync(string? stock);
        public Task Subscribe( string symbol);
        public Task Unsubscribe( string symbol);
        Task<IEnumerable<Stock>> GetWatchlistAsync();
        Task<string?> GetStocksOhlcAsync(string stock, string timespan, string multiplyer);
    }
    public class StocksService : IStocksService
    {
        private readonly IAuthService _authService;
        private readonly string _apiLink;
        private readonly IHttpClientFactory _httpClientFactory;
        public StocksService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _apiLink = configuration["WebApi:Default"]
                            ?? throw new NullReferenceException();
            _httpClientFactory = httpClientFactory;
            _authService = authService;
        }
        public async Task<IEnumerable<Stock>> GetWatchlistAsync()
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.GetAsync(_apiLink + $"/api/users/{_authService.Login}/subs");
            if (!response.IsSuccessStatusCode)
                return new List<Stock>();   
            var result = JsonConvert.DeserializeObject<IEnumerable<Stock>>(await response.Content.ReadAsStringAsync());
            return result;
        }
        public async Task Unsubscribe(string symbol)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.DeleteAsync(_apiLink + $"/api/users/{_authService.Login}/subs/unsubscribe?ticker={symbol}");
        }

        public async Task Subscribe(string symbol)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.PostAsync(_apiLink + $"/api/users/{_authService.Login}/subs/subscribe?ticker={symbol}", null);
        }

        public async Task<IEnumerable<StocksPreview>> SearchAsync(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return Enumerable.Empty<StocksPreview>();
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.GetAsync(_apiLink + $"/api/stocks/search?input={input}");
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<StocksPreview>();
            var result = JsonConvert.DeserializeObject<IEnumerable<StocksPreview>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<Stock?> GetStockInfoAsync(string? stock)
        {
            if (string.IsNullOrEmpty(stock))
                return null;
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.GetAsync(_apiLink + $"/api/stocks/{stock}");
            var result = JsonConvert.DeserializeObject<Stock>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<string?> GetStocksOhlcAsync(string stock, string timespan, string multiplyer)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await client.GetAsync(_apiLink + $"/api/stocks/{stock}/ohlc?multiplier={multiplyer}&timespan={timespan}&from=2023-06-07&to=2023-06-10&sort=asc&limit=3000");
            Console.WriteLine(response.StatusCode);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
