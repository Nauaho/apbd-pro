namespace WebApi.Models.DTOs
{
    public class SearchResultsDTO
    {
        public int count { get; set; }

        public string? next_Url { get; set; }
        public string? request_Id { get; set; }
        public IEnumerable<StockPreviewDTO> results { get; set; } = null!;
        public string? status { get; set; }
    }
}
