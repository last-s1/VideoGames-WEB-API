namespace VideoGamesAPI.Models
{
    public class ResponseMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Metadata { get; set; } = string.Empty;
    }
}
