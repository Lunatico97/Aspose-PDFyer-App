using Aspose.Pdf;
using Aspose.Pdf.Text;
using AsposeTriage.Structures;

namespace AsposeTriage.Services.Interfaces
{
    public interface IPDFGenerator
    {
        public void CreateHeader(Header iheader, FontStyles style = FontStyles.Regular);
        public void CreateFooter(Footer ifooter);
        public void CreateTextBox(Textbox textbox, FontStyles style = FontStyles.Regular, bool underlined = false);
        public void CreateSection(Textbox textbox);
        public void CreateImage(PDFImage inputImage);
        public void CreateTableFromStringRows(TableData tabledata, Color headercolor, Color rowcolor, bool enableBorder = false);
        public void CreateWatermark(string imageName, double angle, double opacity);
        public void SetBackgroundColor(Color color);
        public void SetForegroundColor(Color color);
        public double GetPageWidth();
        public double GetPageHeight();
        public void GeneratePDF(string filename);
        public void Dispose();
    }
}
