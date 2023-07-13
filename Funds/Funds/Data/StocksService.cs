using Newtonsoft.Json;
using WebApi.Models.DTOs;

namespace Funds.Data
{

    public interface IStocksService
    {

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

        public async Task<IEnumerable<Stock>> GetWatchlist(string login, string token) 
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            using var response = await client.GetAsync(_apiLink + $"/api/users/{login}/subs");
            var result = JsonConvert.DeserializeObject<IEnumerable<Stock>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            return "TODO";
        }
    }
}
