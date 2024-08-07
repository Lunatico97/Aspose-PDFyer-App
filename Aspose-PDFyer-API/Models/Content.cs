using AsposeTriage.Structures;

namespace AsposeTriage.Models
{
    public class Content
    {
        public string Font { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public TableData Table { get; set; } = new TableData();
    }
}