using Azure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Runtime.Intrinsics;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Repositories;

namespace WebApi.Services
{
    public interface IStocksService
    {
        public Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan);
        public Task<IEnumerable<StocksPreview>> GetSearchResultsAsync(string input);
        public Task<TickerDetails?> GetTickersDetailsAsync(string ticker, DateOnly date);
        public Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date);
    }
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _v1 = null!;
        private readonly string _v2 = null!;
        private readonly string _v3 = null!;
        private readonly string _apiKey = null!;

        public StocksService(IConfiguration configuration, IStocksRepository stocksRepository, IHttpClientFactory httpClientFactory)
        {
            _v1 = configuration["PolygonAPI:Url1"] ?? throw new NullReferenceException();
            _v2 = configuration["PolygonAPI:Url2"] ?? throw new NullReferenceException();
            _v3 = configuration["PolygonAPI:Url3"] ?? throw new NullReferenceException();
            _apiKey = configuration["PolygonAPI:Default"] ?? throw new NullReferenceException();
            _stocksRepository = stocksRepository;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<string?> GetImage(string? Url)
        {
            if (Url == null)
                return null;
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(Url+ $"?apiKey={_apiKey}");
            response.EnsureSuccessStatusCode();
            var result = Convert.ToBase64String(await response.Content.ReadAsByteArrayAsync());
            return result;
        }

        public async Task<TickerDetails?> GetTickersDetailsAsync(string ticker, DateOnly date)
        {
            try
            {
                using HttpClient client = _httpClientFactory.CreateClient();
                //Console.WriteLine(_v3 + $"reference/tickers/{ticker}?date={date.ToString("yyyy-MM-dd")}&apiKey={_apiKey}");
                var response = await client.GetAsync(_v3 + $"reference/tickers/{ticker}?date={date:yyyy-MM-dd}&apiKey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<WebApi.Models.Response>( await response.Content.ReadAsStringAsync());
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
                    Address = result.results.address?.address1,
                    City = result.results.address?.city,
                    State = result.results.address?.state,
                    PostalCode = result.results.address?.postal_code,
                    Description = result.results.description,
                    SicCode = result.results.sic_code,
                    SicDescription = result.results.sic_description,
                    TickerRoot = result.results.ticker_root,
                    HomepageUrl = result.results.homepage_url,
                    TotalEmployees = result.results.total_employees,
                    ListDate = result.results.list_date,
                    LogoUrl = await GetImage(result.results.branding?.logo_url),
                    IconUrl = await GetImage(result.results.branding?.icon_url),
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
        }

        public async Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date)
        {
            try
            {
                using HttpClient client = _httpClientFactory.CreateClient();
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
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan)
        {
            List<TickerOHLC> ohlc = new();
            try
            {
                var a = await _stocksRepository
                    .GetAggregationAsync(ticker,
                                         multiplier,
                                         timespan
                                         );
                string nextUrl = _v2 + $"aggs/ticker/{ticker}/range/{multiplier}/{timespan}/{DateTime.Now.AddYears(-1):yyyy-MM-dd}/{DateTime.Now.Date:yyyy-MM-dd}?adjusted=false&sort=asc&limit=50000&apiKey={_apiKey}";
                if (a is not null)
                    ohlc = ohlc.Union(a).ToList();
                do
                {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync(nextUrl);
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<TickerOhlcDTO>(await response.Content.ReadAsStringAsync());
                    nextUrl = result.next_url;
                    ohlc.AddRange(result.results.Select(b => new TickerOHLC()
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
                    }));
                    await _stocksRepository.AddTickerOHLCAsync(ohlc);
                }
                while (nextUrl is not null);
                return ohlc;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return ohlc;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<StocksPreview>> GetSearchResultsAsync(string input)
        {
            try 
            {
                input = input.ToUpper();
                using var client = _httpClientFactory.CreateClient();
                var responseTask = client.GetAsync(_v3 + $"reference/tickers?ticker.gte={input}&active=true&apiKey={_apiKey}");
                var repoSearchTask = _stocksRepository.SearchAsync(input);
                await Task.WhenAll(responseTask, repoSearchTask);
                var response = responseTask.Result;
                SearchResultsDTO result;
                var repoSearch = repoSearchTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<SearchResultsDTO>(await response.Content.ReadAsStringAsync());
                    await _stocksRepository.AddManyTickerDetailsAsyncIfNotExists(result.results);
                    var resultWithIcons = _stocksRepository.SearchIcons(result.results);
                    return resultWithIcons.Union(repoSearch);
                }
                else return repoSearch;
                
            }
            catch (HttpRequestException)
            {
                return await _stocksRepository.SearchAsync(input);
            }
            catch (Exception)
            {
                return Enumerable.Empty<StocksPreview>();
            }
        }
    }
}
