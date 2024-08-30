namespace AsposeTriage.Models
{
    public class ResponseMessage
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
