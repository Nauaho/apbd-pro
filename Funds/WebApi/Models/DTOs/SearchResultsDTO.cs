namespace WebApi.Models.DTOs
{
    public class SearchResultsDTO
    {
        public int Count { get; set; }

        public string? Next_Url { get; set; }
        public string? Request_Id { get; set; }
        public IEnumerable<StockPreviewDTO> Results { get; set; } = Enumerable.Empty<StockPreviewDTO>();
        public string? Status { get; set; }
    }
}
