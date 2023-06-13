using System.Runtime.Intrinsics;
using WebApi.Models;
using WebApi.Models.DTOs;

namespace WebApi.Services
{
    public interface IStocksService
    {
        Task<TickerDetailsDTO?> GetTickersDetailsAsync(string ticker, DateOnly date);
    }
    public class StocksService : IStocksService
    {
        private readonly string _v1 = null!;
        private readonly string _v2 = null!;
        private readonly string _v3 = null!;
        private readonly string _apiKey = null!;

        public StocksService(IConfiguration configuration) 
        {
            _v1 = configuration["PolygonAPI:Url1"] ?? throw new NullReferenceException();
            _v2 = configuration["PolygonAPI:Url2"] ?? throw new NullReferenceException();
            _v3 = configuration["PolygonAPI:Url3"] ?? throw new NullReferenceException();
            _apiKey = configuration["PolygonAPI:Default"] ?? throw new NullReferenceException();
        }

        public async Task<TickerDetailsDTO?> GetTickersDetailsAsync(string ticker, DateOnly date)
        {
            try
            {
                using HttpClient client = new();
                //Console.WriteLine(_v3 + $"reference/tickers/{ticker}?date={date.ToString("yyyy-MM-dd")}&apiKey={_apiKey}");
                var response = await client.GetAsync(_v3 + $"reference/tickers/{ticker}?date={date.ToString("yyyy-MM-dd")}&apiKey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<Response?>();
                var tickerDetails = new TickerDetails() 
                {
                    Ticker = 
                };
                return result == null ? null : result.results;
            }
            catch (HttpRequestException e) 
            {
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }

            //throw new NotImplementedException();
        }

        public async Task<TickerOpenClose> GetTickersOpenCloseAsync(string ticker, DateOnly date)
        {
            throw new NotImplementedException();
        }

        public async Task<TickerDetailsDTO> GetTickersDetailsAsync(string Ticker, int queryCount)
        {
            throw new NotImplementedException();
        }
    }
}
