﻿using System.Runtime.Intrinsics;
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
                var result = await response.Content.ReadAsAsync<Response?>() ?? throw new NullReferenceException();
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
                    Address = new Localisation()
                    {
                        Address = result.results.address.address1,
                        City = result.results.address.city,
                        State = result.results.address.state,
                        PostalCode = result.results.address.postal_code
                    },
                    Description = result.results.description,
                    SicCode = result.results.sic_code,
                    SicDescription = result.results.sic_description,
                    TickerRoot = result.results.ticker_root,
                    HomepageUrl = result.results.homepage_url,
                    TotalEmployees = result.results.total_employees,
                    ListDate = result.results.list_date,
                    Branding = new Branding()
                    {
                        LogoUrl = result.results.branding.logo_url,
                        IconUrl = result.results.branding.icon_url
                    },
                    ShareClassSharesOutstanding = result.results.share_class_shares_outstanding,
                    WeightedSharesOutstanding = result.results.weighted_shares_outstanding,
                    RoundLot = result.results.round_lot
                };
                return result == null ? null : result.results;
            }
            catch (HttpRequestException e) 
            {
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
            catch(NullReferenceException  e) 
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
