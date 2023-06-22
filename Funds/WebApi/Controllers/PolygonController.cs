using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTOs;
using WebApi.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/stocks/")]
    [ApiController]
    [Authorize]
    public class PolygonController : Controller
    {
        private readonly IStocksService _stocksService;
        public PolygonController(IStocksService stocksService) 
        {
            _stocksService = stocksService; 
        }

        [HttpGet("{ticker}")]
        public async Task<IActionResult> GetDetails(string ticker)
        {
            var t = await _stocksService.GetTickersDetailsAsync(ticker, DateOnly.FromDateTime(DateTime.Now));
            if (t == null)
                return NotFound();
            var result = new TickerDTO
            {
                Name = t.Name,
                Market = t.Market,
                Locale = t.Locale,
                PrimaryExchange = t.PrimaryExchange,
                Type = t.Type,
                Active = t.Active,
                CurrencyName = t.CurrencyName,
                Cik = t.Cik,
                CompositeFigi = t.CompositeFigi,
                ShareClassFigi = t.ShareClassFigi,
                PhoneNumber = t.PhoneNumber,
                Address = t.Address,
                City = t.City,
                State = t.State,
                PostalCode = t.PostalCode,
                Description = t.Description,
                SicCode = t.SicCode,
                SicDescription = t.SicDescription,
                TickerRoot = t.TickerRoot,
                HomepageUrl = t.HomepageUrl,
                TotalEmployees = t.TotalEmployees,
                ListDate = t.ListDate,
                LogoUrl = t.LogoUrl,
                IconUrl = t.IconUrl,
                ShareClassSharesOutstanding = t.ShareClassSharesOutstanding,
                WeightedSharesOutstanding = t.WeightedSharesOutstanding,
                RoundLot = t.RoundLot,
                Ticker = t.Ticker,
                Similar = t.Similar.Select(s => s.TickerTwo).ToList(),
            };
            return Ok(result);
        }

        [HttpGet("{ticker}/ohlc")]
        public async Task<IActionResult> GetOHLC(string ticker,int multiplier, string timespan, string from, string to, string sort, long limit)
        {
            var ohlc = await _stocksService.GetAggregationAsync(ticker, multiplier, timespan, DateOnly.ParseExact(from, "yyyy-MM-dd"), DateOnly.ParseExact(to, "yyyy-MM-dd"), sort, limit);
            if (ohlc == null || !ohlc.Any())
                return NotFound();
            var result = ohlc.Select(o => new OhlcDTO
            {
                C = o.C,
                H = o.H,
                L = o.L,
                N = o.N,
                O = o.O,
                V = o.V,
                Vw = o.Vw,
                T = o.T,
                Multuplier = o.Multuplier,
                Timespan = o.Timespan,
                Symbol = o.Symbol
            });
            return Ok(result);
        }

        [HttpGet("{ticker}/openclose")]
        public async Task<IActionResult> GetOpenClose(string ticker, string date)
        {
            var oc = await _stocksService.GetTickersOpenCloseAsync(ticker, DateOnly.ParseExact(date, "yyyy-MM-dd"));
            if (oc == null) 
                return NotFound();
            var result = new DailyOpenCloseDTO 
            {
                AfterHours = oc.AfterHours,
                Close = oc.Close,
                High = oc.High,
                Low = oc.Low,  
                Open = oc.Open,
                PreMarket = oc.PreMarket,
                Volume = oc.Volume,
                From = DateOnly.FromDateTime(oc.From),
                Symbol = oc.Symbol
            };
            return Ok(result);
        }

    }
}
