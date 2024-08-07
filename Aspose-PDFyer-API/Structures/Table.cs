namespace AsposeTriage.Structures
{
    public class TableData
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int FontSize { get; set; } = 10;
        public int Padding { get; set; } = 2;
        public string ColumnWidths { get; set; } = string.Empty;
        public string[] HeaderRows { get; set; } = [];
        public List<string[]> DataRows { get; set; } = new List<string[]>();
        public bool AutoFit = false;
    }
}
