using Aspose.Pdf;
// This loads general element objects like document, page and rectangle.
using Aspose.Pdf.Text;
using AsposeTriage.Common;

// This loads specific text-associated document elements like TextFragment, header, fonts, etc...
using AsposeTriage.Models;
using AsposeTriage.Structures;

namespace AsposeTriage.Utilities
{
    public static class PDFManipulator
    {
        public static void Generate(Header iheader, Content icontent, string filename)
        {
            Document document = new Document();
            Page page = document.Pages.Add();
            page.Background = Color.Black;
            // Creating header for PDF
            var header = new TextFragment(); 
            header.Text = iheader.Title;
            header.TextState.ForegroundColor = Color.White;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.VerticalAlignment = VerticalAlignment.Top;
            header.TextState.Font = FontRepository.FindFont(iheader.Font);
            header.TextState.FontSize = 20;
            header.Position = new Position(130, 720);
            page.Paragraphs.Add(header);
            // Adding image to PDF
            using var stream = System.IO.File.OpenRead(iheader.ImagePath);
            {
                page.AddImage(stream, new Rectangle(20, 730, 120, 830));
            }
            // Add content text to the page
            var content = new TextFragment(icontent.Text);
            content.TextState.Font = FontRepository.FindFont(icontent.Font);
            content.TextState.FontSize = 14;
            content.TextState.ForegroundColor = Color.White;
            content.HorizontalAlignment = HorizontalAlignment.Left;
            content.VerticalAlignment = VerticalAlignment.Center;
            page.Paragraphs.Add(content);
            // Add table to the page
            var table = new Table();
            table.BackgroundColor = Color.AliceBlue;
            table.Border = new BorderInfo(BorderSide.Box, 1.0f, Color.Black);
            table.HorizontalAlignment = HorizontalAlignment.Center;
            table.VerticalAlignment = VerticalAlignment.Center;
            page.Paragraphs.Add(table);
            var headerrow = table.Rows.Add();
            headerrow.BackgroundColor = Color.Orange;
            foreach(var h in icontent.Table.HeaderRows)
            {
                headerrow.Cells.Add(h);
            }
            foreach(var row in icontent.Table.DataRows)
            {
                var datarow = table.Rows.Add();
                datarow.Border = new BorderInfo(BorderSide.Bottom, 1.0f, Color.Gray);
                for (var i=0; i<row.Length; i++)
                {
                    datarow.Cells.Add(row[i]);
                }
                
            }
            // Save updated PDF
            var outputFileName = System.IO.Path.Combine(Defaults.DispatchDirectory, $"{filename}.pdf");
            document.Save(outputFileName);
        }

        public static List<string[]> ParseTableFromPDF(IFormFile file)
        {
            List<string[]> parsedTable = new List<string[]>();
            Document document = new Document(file.OpenReadStream());
            foreach(var page in document.Pages)
            {
                TableAbsorber absorber = new TableAbsorber();
                absorber.Visit(page);
                foreach(var table in absorber.TableList)
                {
                    foreach (var rows in table.RowList) 
                    {
                        foreach (AbsorbedCell cell in rows.CellList)
                        {
                            string[] parsedRow = new string[cell.TextFragments.Count()];
                            var i = 0;
                            foreach (TextFragment fragment in cell.TextFragments)
                            {
                                parsedRow[i] = fragment.Text;
                                i++;
                                continue;
                            }
                            parsedTable.Add(parsedRow);
                        } 
                    }
                }
            }
            return parsedTable;
        }
       
        public static void GenerateUsingXML(string inXML, string filename)
        {
            Document document = new Document();
            try
            {
                document.BindXml($"{Defaults.UploadDirectory}/{inXML}.xml", $"{Defaults.UploadDirectory}/{inXML}.xslt");
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
            var outputFileName = System.IO.Path.Combine(Defaults.DispatchDirectory, $"{filename}.pdf");
            document.Save(outputFileName);
        }

    }
}
