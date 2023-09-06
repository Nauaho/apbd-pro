using Microsoft.JSInterop;

namespace Funds.Data
{
    public interface IStockChartService
    {
        public Task ApplyStyleOnElementAsync(string idElement, string style);
        public Task<bool> DrawMeChartAsync(string stock, string timespan, string multiplyer, string idOfChartsDiv);
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
        public async Task<bool> DrawMeChartAsync(string stock, string timespan, string multiplyer, string idOfChartsDiv)
        {
            var data = await _stocksService.GetStocksOhlcAsync(stock, timespan, multiplyer);
            if(data is null)
                return false;
            await _jSRuntime.InvokeVoidAsync("createChart", data, idOfChartsDiv);
            return true;
        }

        public async Task ApplyStyleOnElementAsync(string idElement, string style)
        {
            await _jSRuntime.InvokeVoidAsync("applyStyle", idElement, style);
        }

        public async Task ApplyClassAsync(string cssClass, string style)
        {
            await _jSRuntime.InvokeVoidAsync("applyClass", cssClass, style);
        }
    }
}
