namespace AsposeTriage.Structures
{
    public class PDFImage
    {
        public string Filename { get; set; } = string.Empty;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PDFImage(string filename, int posX, int posY, int width, int height)
        {
            Filename = filename;
            PosX = posX;
            PosY = posY;
            Width = width;
            Height = height;
        }
    }
}
