using System.Runtime.Intrinsics;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IStocksService
    {
        Task<TickerDetails?> GetTickersDetailsAsync(string ticker);
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
            _apiKey = configuration["PolygonAPI:Key"] ?? throw new NullReferenceException();
        }

        public async Task<TickerDetails?> GetTickersDetailsAsync(string ticker)
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(_v1 + $"/reference/tickers/{ticker}?{_apiKey}");
                response.EnsureSuccessStatusCode();
                var details = await response.Content.ReadAsAsync<TickerDetails>();
                return details;
            }
            catch (HttpRequestException e) 
            {
                if (e.HResult == 404)
                    return null;
                else
                    return new TickerDetails();
            }

            //throw new NotImplementedException();
        }

        public async Task<TickerOpenClose> GetTickersOpenCloseAsync(string ticker, DateOnly date)
        {
            throw new NotImplementedException();
        }

        public async Task<TickerDetails> GetTickersDetailsAsync(string Ticker, int queryCount)
        {
            throw new NotImplementedException();
        }
    }
}
