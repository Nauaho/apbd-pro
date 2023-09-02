using Funds.Data;
using Microsoft.AspNetCore.Components;
using Funds.Models;

namespace Funds.Shared
{
    public abstract partial class StockList : ComponentBase
    {
        [Inject]
        public IStocksService StocksService { get; set; } = default(StocksService)!;
        protected virtual IEnumerable<StocksPreview> StocksToShow { get; set; } = new List<StocksPreview>();
        protected virtual IEnumerable<StocksPreview> Stocks { get; set; } = new List<StocksPreview>();
        protected virtual IDictionary<string, string> IsColumnsAreSorted { get; set; } = new Dictionary<string, string>();

        protected virtual async Task Unsubscribe(string ticker)
        {
            await StocksService.Unsubscribe(ticker);
            Stocks = Stocks.Where(s => s.Ticker != ticker);
            StateHasChanged();
        }

        protected virtual async Task Subscribe(StocksPreview stock)
        {
            await StocksService.Subscribe(stock.Ticker);
            Stocks = Stocks.Append(stock);
            StateHasChanged();
        }

        protected virtual Task GetStocksToShowAsync()
        {
            StocksToShow = new List<StocksPreview>();
            return Task.FromResult(StocksToShow);
        }

        protected virtual Task GetStocksAsync()
        {
            Stocks = new List<StocksPreview>();
            return Task.FromResult(StocksToShow);
        }
        protected override async Task OnInitializedAsync()
        {
            IsColumnsAreSorted = new Dictionary<string, string>()
        {
            { "Logo", "Uncertain" },
            { "Symbol", "Uncertain" },
            { "Name", "Uncertain" },
            { "Country", "Uncertain" },
        };
            await Task.WhenAll
            (
                GetStocksAsync(),
                GetStocksToShowAsync()
            );

            base.OnInitialized();
            ShouldRender();
        }

        protected virtual Task SortBy(string column)
        {
            var c = IsColumnsAreSorted[column];
            if (c is null) return Task.CompletedTask;
            if (c == "Sorted")
            {
                StocksToShow = StocksToShow.OrderByDescending(x => x.YieldProps()[column]);
                IsColumnsAreSorted[column] =  "Sorted Reversed";
            }
            else
            {
                StocksToShow = StocksToShow.OrderBy(x => x.YieldProps()[column]);
                IsColumnsAreSorted[column] = "Sorted";
            }
            return Task.FromResult(StocksToShow);
        }
    }
}
