using Aspose.Cells;
using Aspose.Pdf;
using AsposeTriage.Structures;
using AsposeTriage.Models;
using AsposeTriage.Common;

namespace AsposeTriage.Services
{
    public class WWECreator
    {
        private readonly IPDFGenerator _generator;
        private List<Wrestler> _wrestlers = new List<Wrestler>();
        private Wrestler? _selectedWrestler1, _selectedWrestler2;
        List<string[]> tabularData = new List<string[]>();
        public WWECreator(IPDFGenerator generator)
        {
            _generator = generator;
        }

        public List<Wrestler> GetRosterData()
        {
            Workbook sheets = new Workbook($"{Defaults.WWEResourcePath}/{Defaults.WrestlerDataFile}");
            Worksheet worksheet = sheets.Worksheets[1];
            List<Wrestler> rows = new List<Wrestler>();
            Aspose.Cells.Cells cells = worksheet.Cells;
            for (int row = 1; row <= cells.MaxDataRow; row++)
            {
                Wrestler wrestler = new Wrestler{
                    Name = cells[row, 0].StringValue,
                    Division = cells[row, 1].StringValue,
                    Weight = Convert.ToInt32(cells[row, 2].StringValue),
                    Finisher = cells[row, 3].StringValue,
                    Profile = cells[row, 4].StringValue,
                };
                rows.Add(wrestler);
            }
            sheets.Dispose();
            return rows;
        }

        public void CreateRoster()
        {
            _wrestlers = this.GetRosterData();
            foreach (Wrestler w in _wrestlers)
            {
                tabularData.Add(new[]
                {
                    w.Name, w.Division, w.Weight.ToString(), w.Finisher
                });
            }
        }

        public void CreateMatchCard(string wrestler1, string wrestler2)
        {
            _selectedWrestler1 = _wrestlers.FirstOrDefault(w => w.Name.Equals(wrestler1, StringComparison.OrdinalIgnoreCase));
            _selectedWrestler2 = _wrestlers.FirstOrDefault(w => w.Name.Equals(wrestler2, StringComparison.OrdinalIgnoreCase));
            if (_selectedWrestler1 == null || _selectedWrestler2 ==  null)
                throw (new Exception(Messages.WrestlerNotInRoster));
        }

        public void RenderCard()
        {
            if (_selectedWrestler1 == null || _selectedWrestler2 == null) return;
            // Banner & Header
            _generator.SetBackgroundColor(Color.Black);
            //_generator.CreateWatermark($"{RsrcURL}/wwe.jpg", 0, 0.75F);
            _generator.SetForegroundColor(Color.FloralWhite);
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{Defaults.SurvivorSeriesLogoFile}", 0,(int)_generator.GetPageHeight()-40, (int)_generator.GetPageWidth(), 40));
            _generator.CreateHeader(new Header
            {
                Font = "Times New Roman", Title = "MATCH CARD", FontSize = 20, Top = 100
            });
            _generator.SetForegroundColor(Color.DarkGoldenrod);
            _generator.CreateTextBox(new Textbox([$"{_selectedWrestler1.Name}"], "Times New Roman", 100, 50, 100, 20, 18));
            _generator.CreateTextBox(new Textbox([$"{_selectedWrestler2.Name}"], "Times New Roman", 380, -18, 100, 20, 18));
            
            // Wrestler 1
            _generator.SetForegroundColor(Color.FloralWhite);
            _generator.CreateSection(new Textbox(
              [$"Weight: {_selectedWrestler1.Weight} lbs", $"Finisher: {_selectedWrestler1.Finisher}"],
              "Calibri", 80, 450, 300, 100, 18
            ));
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{_selectedWrestler1.Profile}", 80, 650, 150, 250));
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{_selectedWrestler1.Division}.png", 80, 350, 100, 50));

            // Wrestler 2
            _generator.CreateSection(new Textbox(
             [$"Weight: {_selectedWrestler2.Weight} lbs", $"Finisher: {_selectedWrestler2.Finisher}"], 
             "Calibri", 380, 450, 300, 100, 18
           ));
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{_selectedWrestler2.Profile}", 380, 650, 150, 250));
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{_selectedWrestler2.Division}.png", 380, 350, 100, 50));

            // Mascot & Footer
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{Defaults.WWELogoFile}", 450, 120, 100, 100));
            _generator.CreateImage(new PDFImage($"{Defaults.WWEResourcePath}/{Defaults.MascotImageFile}", 50, 180, 100, 200));
            _generator.SetForegroundColor(Color.Red);
            _generator.CreateFooter(new Footer
            {
                Font = "Times New Roman",
                Text = "Buy your tickets now !",
                Link = "https://wwe.com/tickets",
                FontSize = 15
            });
        }

        public string? GenerateCard()
        {
            if (_selectedWrestler1 == null || _selectedWrestler2 == null) return null;
            var filename = $"{_selectedWrestler1.Name} vs {_selectedWrestler2.Name}.pdf";
            _generator.GeneratePDF($"{Defaults.DispatchDirectory}/{filename}");
            _generator.Dispose();
            return filename;
        }

        /*
         _generator.SetBackgroundColor(Color.Black);
                _generator.SetForegroundColor(Color.White);
                _generator.CreateHeader(new Header
                {
                    Font = "Times New Roman",
                    Title = "WWE Roster",
                    FontSize = 16,
                }) ;
                _generator.CreateImage(new PDFImage("Input/wwe.jpg", 20, 700, 100, 100));
                _generator.CreateTableFromStringRows(new TableData
                {
                    PosX = 100, PosY = 150,
                    HeaderRows = new[] { "Name", "Division", "Weight (pounds)", "Finisher" },
                    DataRows = SheetManipulator.GetRowsFromExcel("Input/Wrestlers")
                }, Color.Orange, Color.AliceBlue, true);
                _generator.SetForegroundColor(Color.Red);
                _generator.CreateFooter(new Footer
                {
                    Font = "Times New Roman",
                    Text = "Buy your tickets now !",
                    Link = "wwe.com/tickets"
                });
                _generator.GeneratePDF("Output/Roster.pdf");
                _generator.Dispose();*/
    }
}
