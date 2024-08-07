namespace AsposeTriage.Models
{
    public class CustomDAO
    {
        public string Title { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty ;
        public string Filename { get; set; } = string.Empty;
        public string[] Headers { get; set; } = [];
        public int RelativeTableX { get; set; } = 30;
        public int RelativeTableY { get; set; } = 50;
        public int TableFontSize { get; set; } = 8;
    }
}
