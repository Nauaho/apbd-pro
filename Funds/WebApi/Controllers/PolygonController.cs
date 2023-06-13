using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var details = await _stocksService.GetTickersDetailsAsync(ticker, DateOnly.FromDateTime(DateTime.Now));
            if(details == null)
                return NotFound();
            else return Ok(details);
        }

        [HttpGet("{ticker}/ohlc")]
        public async Task<IActionResult> GetOHLC(string ticker,int multiplier, string timespan, string from, string to, string sort, long limit)
        {
            var ohlc = await _stocksService.GetAggregationAsync(ticker, multiplier, timespan, DateOnly.ParseExact(from, "yyyy-MM-dd"), DateOnly.ParseExact(to, "yyyy-MM-dd"), sort, limit);
            if (ohlc == null)
                return NotFound();
            else
                return Ok(ohlc);
        }

        [HttpGet("{ticker}/openclose")]
        public async Task<IActionResult> GetOpenClose(string ticker, string date)
        {
            var oc = await _stocksService.GetTickersOpenCloseAsync(ticker, DateOnly.ParseExact(date, "yyyy-MM-dd"));
            if (oc == null) 
                return NotFound();
            else
                return Ok(oc);
        }

    }
}
