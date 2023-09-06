using Funds.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

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
            using var response = await GetResourceAsync($"/api/users/{_authService.Login}/subs", RestMethod.Get);
            if (!response.IsSuccessStatusCode)
                return new List<Stock>();   
            var result = JsonConvert.DeserializeObject<IEnumerable<Stock>>(await response.Content.ReadAsStringAsync());
            return result;
        }
        public async Task Unsubscribe(string symbol)
        {
            using var response = await GetResourceAsync($"/api/users/{_authService.Login}/subs/unsubscribe?ticker={symbol}", RestMethod.Delete);
        }

        public async Task Subscribe(string symbol)
        {
            using var response = await GetResourceAsync($"/api/users/{_authService.Login}/subs/subscribe?ticker={symbol}", RestMethod.Post);
        }

        public async Task<IEnumerable<StocksPreview>> SearchAsync(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return Enumerable.Empty<StocksPreview>();
            using var response = await GetResourceAsync($"/api/stocks/search?input={input}", RestMethod.Get);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<StocksPreview>();
            var result = JsonConvert.DeserializeObject<IEnumerable<StocksPreview>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<Stock?> GetStockInfoAsync(string? stock)
        {
            if (string.IsNullOrEmpty(stock))
                return null;
            using var response = await GetResourceAsync($"/api/stocks/{stock}", RestMethod.Get);
            var result = JsonConvert.DeserializeObject<Stock>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<string?> GetStocksOhlcAsync(string stock, string timespan, string multiplyer)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            using var response = await GetResourceAsync($"/api/stocks/{stock}/ohlc?multiplier={multiplyer}&timespan={timespan}&from=2023-06-07&to=2023-06-10&sort=asc&limit=3000", RestMethod.Get);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(string endpoint, RestMethod method, HttpContent? content)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            HttpResponseMessage response = method switch
            {
                RestMethod.Get => await client.GetAsync(_apiLink + endpoint),
                RestMethod.Post => await client.PostAsync(_apiLink + endpoint, content),
                RestMethod.Put => await client.PutAsync(_apiLink + endpoint, content),
                RestMethod.Delete => await client.DeleteAsync(_apiLink + endpoint),
                _ => null!,
            };
            return response;
        }

        private async Task<HttpResponseMessage> GetResourceAsync(string endpoint, RestMethod method, HttpContent? content = null)
        {
            var firstResponse = await SendHttpRequestAsync(endpoint, method, content);
            if (firstResponse.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                return firstResponse;
            firstResponse.Dispose();
            await _authService.RefreshTheToken();
            var secondResponse = await SendHttpRequestAsync(endpoint, method, content);
            return secondResponse;
        }
    }

    enum RestMethod
    {
        Get = 0, 
        Post = 1,
        Put = 2,
        Delete = 3
    } 
}
