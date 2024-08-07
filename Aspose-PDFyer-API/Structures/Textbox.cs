namespace AsposeTriage.Structures
{
    public class Textbox
    {
        public string[] TextList { get; set; } = [];
        public string Font { get; set; } = string.Empty;
        public int FontSize { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Textbox(string[] textList, string font, int x, int y, int w, int h, int pt = 14)
        {
            TextList = textList;
            Font = font; FontSize = pt;
            PosX = x; PosY = y; Width = w; Height = h;
        }
    }
}
