namespace AsposeTriage.Models
{
    public class Sales
    {
        public string ID { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty ;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
