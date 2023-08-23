using Microsoft.JSInterop;
using System.Text.Json;

namespace Funds.Data
{
    public interface IStockChartService
    {
        public Task DrawMeChartAsync(string stock, string timespan, string multiplyer, string idOfChartsDiv);
    }
    public class StockChartService : IStockChartService
    {
        private readonly IStocksService _stocksService;
        private readonly IJSRuntime _jSRuntime;
        public StockChartService(IStocksService stocksService, IJSRuntime jSRuntime)
        {
            _stocksService = stocksService;
            _jSRuntime = jSRuntime;
        }
        public async Task DrawMeChartAsync(string stock, string timespan, string multiplyer, string idOfChartsDiv)
        {
            Console.WriteLine("Creating Chart");
            var data = await _stocksService.GetStocksOhlcAsync(stock, timespan, multiplyer);
            await _jSRuntime.InvokeVoidAsync("createChart", data, "#"+idOfChartsDiv);
            
            //await _jSRuntime.InvokeVoidAsync("eval", "2+2");
        }
    }
}
