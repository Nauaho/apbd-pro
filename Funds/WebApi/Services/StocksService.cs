using Newtonsoft.Json;
using System.Runtime.Intrinsics;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Repositories;

namespace WebApi.Services
{
    public interface IStocksService
    {
        public Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan, DateOnly from, DateOnly to, string sort, long limit);
        public Task<TickerDetails?> GetTickersDetailsAsync(string ticker, DateOnly date);
        public Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date);
    }
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly string _v1 = null!;
        private readonly string _v2 = null!;
        private readonly string _v3 = null!;
        private readonly string _apiKey = null!;

        public StocksService(IConfiguration configuration, IStocksRepository stocksRepository)
        {
            _v1 = configuration["PolygonAPI:Url1"] ?? throw new NullReferenceException();
            _v2 = configuration["PolygonAPI:Url2"] ?? throw new NullReferenceException();
            _v3 = configuration["PolygonAPI:Url3"] ?? throw new NullReferenceException();
            _apiKey = configuration["PolygonAPI:Default"] ?? throw new NullReferenceException();
            _stocksRepository = stocksRepository;
        }

        public async Task<TickerDetails?> GetTickersDetailsAsync(string ticker, DateOnly date)
        {
            try
            {
                using HttpClient client = new();
                //Console.WriteLine(_v3 + $"reference/tickers/{ticker}?date={date.ToString("yyyy-MM-dd")}&apiKey={_apiKey}");
                var response = await client.GetAsync(_v3 + $"reference/tickers/{ticker}?date={date:yyyy-MM-dd}&apiKey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<Response>( await response.Content.ReadAsStringAsync());
                var tickerDetails = new TickerDetails()
                {
                    Ticker = result.results.ticker,
                    Name = result.results.name,
                    Market = result.results.market,
                    Locale = result.results.locale,
                    PrimaryExchange = result.results.primary_exchange,
                    Type = result.results.type,
                    Active = result.results.active,
                    CurrencyName = result.results.currency_name,
                    Cik = result.results.cik,
                    CompositeFigi = result.results.composite_figi,
                    ShareClassFigi = result.results.share_class_figi,
                    PhoneNumber = result.results.phone_number,
                    Address = result.results.address == null ? null : result.results.address.address1,
                    City = result.results.address == null ? null : result.results.address.city,
                    State = result.results.address == null ? null : result.results.address.state,
                    PostalCode = result.results.address == null ? null : result.results.address.postal_code,
                    Description = result.results.description,
                    SicCode = result.results.sic_code,
                    SicDescription = result.results.sic_description,
                    TickerRoot = result.results.ticker_root,
                    HomepageUrl = result.results.homepage_url,
                    TotalEmployees = result.results.total_employees,
                    ListDate = result.results.list_date,
                    LogoUrl = result.results.branding == null ? null : result.results.branding.logo_url,
                    IconUrl = result.results.branding == null ? null : result.results.branding.icon_url,
                    ShareClassSharesOutstanding = result.results.share_class_shares_outstanding,
                    WeightedSharesOutstanding = result.results.weighted_shares_outstanding,
                    RoundLot = result.results.round_lot
                };
                await _stocksRepository.AddTickerDetailsAsync(tickerDetails);
                return tickerDetails;
            }
            catch (HttpRequestException) 
            {
                return await _stocksRepository.GetTickersDetailsAsync(ticker);
            }
            catch(NullReferenceException  e) 
            {
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
            //throw new NotImplementedException();
        }

        public async Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetAsync(_v1+ $"open-close/{ticker}/{date:yyyy-MM-dd}?adjusted=false&apiKey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<OpenCloseDTO>(await response.Content.ReadAsStringAsync());
                var oc = new TickerOpenClose 
                {
                    AfterHours = result.afterHours,
                    Close = result.close,
                    From = result.from,
                    High = result.high,
                    Low = result.low,
                    Open = result.open,
                    PreMarket = result.preMarket,
                    Symbol = result.symbol,
                    Volume = result.volume
                };
                await _stocksRepository.AddTickerOpenCloseAsync(oc);
                return oc;
            }
            catch (HttpRequestException)
            {
                return await _stocksRepository.GetTickersOpenCloseAsync(ticker, date);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan, DateOnly from, DateOnly to, string sort, long limit)
        {
            try
            {
                using HttpClient client = new();
                string a = _v2 + $"aggs/ticker/{ticker}/range/{multiplier}/{timespan}/{from:yyyy-MM-dd}/{to:yyyy-MM-dd}?adjusted=false&sort={sort}&limit={limit}&apiKey={_apiKey}";
                var response = await client.GetAsync(a);
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<TickerOhlcDTO>(await response.Content.ReadAsStringAsync());
                Console.WriteLine(result.results.Count());
                var ohlc = result.results.Select(b =>  new TickerOHLC()
                {
                    C = b.c,
                    H = b.h,
                    L = b.l,
                    N = b.n,
                    O = b.o,
                    T = b.t,
                    V = b.v,
                    Vw = b.vw,
                    Multuplier = multiplier,
                    Timespan = timespan,
                    Symbol = result.ticker
                });
                await _stocksRepository.AddTickerOHLCAsync(ohlc);
                return ohlc;
            }
            catch (HttpRequestException)
            {
                return await _stocksRepository.GetAggregationAsync(ticker, multiplier, timespan, from, to);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
        }
    }
}
