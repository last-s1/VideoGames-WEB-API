namespace VideoGamesAPI.Models
{
    /// <summary>
    /// Класс для хранения параеметров HTTP-Response
    /// </summary>
    public class ResponseMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Dictionary<string,string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
