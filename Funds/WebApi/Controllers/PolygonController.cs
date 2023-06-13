using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/stocks/")]
    [ApiController]
    public class PolygonController : Controller
    {
        private readonly IStocksService _stocksService;
        public PolygonController(IStocksService stocksService) 
        {
            _stocksService= stocksService; 
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
        public async Task<IActionResult> GetOHLC(string ticker, int count)
        {
            throw new NotImplementedException();
        }

        [HttpGet("openclose")]
        public async Task<IActionResult> GetOpenClose(string ticker, DateOnly  date)
        {
            throw new NotImplementedException();
        }

    }
}
