using Aspose.Pdf;
using Aspose.Pdf.Text;
using AsposeTriage.Structures;
using AsposeTriage.Common;
using System.Reflection.PortableExecutable;
using AsposeTriage.Services.Interfaces;
using Amazon.S3.Model;

namespace AsposeTriage.Services
{
    public class PDFGenerator : IPDFGenerator, IDisposable
    {
        private readonly Document _document;
        private readonly Page _page;
        private readonly string _fontPath;
        private Color _foregroundColor = Color.Black;
        private bool disposedValue;

        public PDFGenerator()
        {
            _document = new Document();
            _page = _document.Pages.Add();
            _page.SetPageSize(PageSize.A4.Width, PageSize.A4.Height);
            _page.PageInfo.Margin = new MarginInfo { Top = 0, Bottom = 0, Left = 0, Right = 0 };
            _page.Background = Color.White;
            _fontPath = Path.Combine(Directory.GetCurrentDirectory(), Defaults.ResourceDirectory, Defaults.FontPath);
        }

        public void CreateHeader(Header iheader, FontStyles style = FontStyles.Regular)
        {
            TextFragment header = new TextFragment();
            header.TextState.Font = FontRepository.OpenFont(Path.Combine(_fontPath, iheader.Font));
            header.TextState.FontSize = iheader.FontSize;
            header.TextState.FontStyle = style;
            header.Margin.Top = iheader.Top;
            header.TextState.HorizontalAlignment = HorizontalAlignment.Center;
            header.TextState.ForegroundColor = _foregroundColor;
            header.Text = iheader.Title;
            _page.Paragraphs.Add(header);
        }

        public void CreateImage(PDFImage inputImage)
        {
            using var stream = File.OpenRead(inputImage.Filename);
            {
                _page.AddImage(stream, new Rectangle(inputImage.PosX, inputImage.PosY - inputImage.Height,
                                                     inputImage.PosX + inputImage.Width, inputImage.PosY));
            }
        }

        public void CreateTextBox(Textbox textbox, FontStyles style = FontStyles.Regular, bool underlined=false)
        {
            TextFragment fragment = new TextFragment();
            fragment.Text = textbox.TextList[0];
            fragment.TextState.FontSize = textbox.FontSize;
            fragment.TextState.Underline = underlined;
            fragment.TextState.ForegroundColor = _foregroundColor;
            fragment.TextState.Font = FontRepository.OpenFont(Path.Combine(_fontPath, textbox.Font));
            fragment.TextState.FontStyle = style;
            fragment.Margin = new MarginInfo();
            fragment.Margin.Top = textbox.PosY;
            fragment.Margin.Left = textbox.PosX;
            //fragment.Position = new Position(textbox.PosX, textbox.PosY);
            _page.Paragraphs.Add(fragment);
        }

        public void CreateSection(Textbox textbox)
        {
            Table table = new Table();
            //table.ColumnWidths = $"{textbox.Width/2} {textbox.Width/2}"; 
            table.ColumnWidths = $"{textbox.Width}";
            TextState textState = new TextState();
            textState.Font = FontRepository.OpenFont(Path.Combine(_fontPath, textbox.Font));
            textState.ForegroundColor = _foregroundColor;
            textState.FontSize = 14;
            foreach(string text in textbox.TextList)
            {
                Row row = table.Rows.Add();
                Cell cell = row.Cells.Add(text);
                row.DefaultCellTextState = textState;
            }
            table.Left = textbox.PosX;
            table.Top = textbox.PosY;
            _page.Paragraphs.Add(table);
        }

        public void CreateFooter(Footer ifooter)
        {
            var content = new TextFragment(ifooter.Text);
            var length = content.TextState.MeasureString(content.Text);
            content.Position = new Position((_page.PageInfo.Width / 2) - (length / 2), ifooter.Bottom);
            content.TextState.Font = FontRepository.OpenFont(Path.Combine(_fontPath, ifooter.Font));
            content.TextState.FontSize = ifooter.FontSize;
            content.TextState.ForegroundColor = _foregroundColor;
            if (ifooter.Link != string.Empty)
            {
                content.Hyperlink = new WebHyperlink(ifooter.Link);
                content.TextState.Underline = true;
            }
            _page.Paragraphs.Add(content);
        }
        public void CreateTableFromStringRows(TableData tabledata, Color headercolor, Color rowcolor, bool enableBorder=false)
        {
            // Add table to the page
            var table = new Table();
            if(tabledata.ColumnWidths != string.Empty) table.ColumnWidths = tabledata.ColumnWidths;
            table.BackgroundColor = rowcolor;
            if (enableBorder) table.Border = new BorderInfo(BorderSide.Box, 1.0f, Color.Black);
            table.Margin = new MarginInfo { Left = tabledata.PosX, Top = tabledata.PosY };
            table.HorizontalAlignment = HorizontalAlignment.Center;
            table.VerticalAlignment = VerticalAlignment.Center;
            var headerrow = table.Rows.Add();
            headerrow.DefaultCellTextState = new TextState
            {
                FontSize = (tabledata.FontSize + 3),
                BackgroundColor = headercolor
            };
            headerrow.DefaultCellTextState.BackgroundColor = headercolor;
            headerrow.Border = new BorderInfo(BorderSide.Bottom, 4.0f, Color.Black);
            headerrow.DefaultCellPadding = new MarginInfo(tabledata.Padding, tabledata.Padding, tabledata.Padding, tabledata.Padding);
            foreach (var h in tabledata.HeaderRows)
            {
                headerrow.Cells.Add(h);
            }
            foreach (var row in tabledata.DataRows)
            {
                var datarow = table.Rows.Add();
                datarow.DefaultCellTextState = new TextState
                {
                    FontSize = tabledata.FontSize,
                    BackgroundColor = rowcolor,
                }; 
                datarow.DefaultCellPadding = new MarginInfo(tabledata.Padding, tabledata.Padding, tabledata.Padding, tabledata.Padding);
                datarow.Border = new BorderInfo(BorderSide.Bottom, 1.0f, Color.Gray);
                for (var i = 0; i < row.Length; i++)
                {
                    datarow.Cells.Add(row[i]);
                }

            }
            if (tabledata.AutoFit) table.ColumnAdjustment = ColumnAdjustment.AutoFitToContent;
            _page.Paragraphs.Add(table);
        }

        public void CreateWatermark(string imageName, double angle, double opacity)
        {
            ImageStamp imageStamp = new ImageStamp(imageName);
            imageStamp.Background = true;
            imageStamp.XIndent = this.GetPageWidth()/2 - imageStamp.Width/2; 
            imageStamp.YIndent = this.GetPageHeight()/2 - imageStamp.Height/2; 
            imageStamp.RotateAngle = angle; 
            imageStamp.Opacity = opacity;
            _page.AddStamp(imageStamp);
        }

        public double GetPageWidth()
        {
            return _page.PageInfo.Width;
        }

        public double GetPageHeight()
        {
            return _page.PageInfo.Height;
        }

        public void SetBackgroundColor(Color color)
        {
            _page.Background = color;
        }

        public void SetForegroundColor(Color color)
        {
            _foregroundColor = color;
        }

        public void GeneratePDF(string outFilename)
        {
            _document.Save(outFilename);
        }

        public Stream GeneratePDFStream()
        {
            Stream stream = new MemoryStream();
            _document.Save(stream, SaveFormat.Pdf);
            return stream;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _page.Dispose();
                    _document.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
