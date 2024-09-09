using Aspose.Cells;
using Aspose.Pdf;
using AsposeTriage.Structures;
using AsposeTriage.Models;
using AsposeTriage.Common;
using AsposeTriage.Services.Interfaces;

namespace AsposeTriage.Services.Creators
{
    public class WWECreator
    {
        private readonly IPDFGenerator _generator;
        private List<Wrestler> _wrestlers = new List<Wrestler>();
        private Wrestler? _selectedWrestler1, _selectedWrestler2;
        private List<string[]> tabularData = new List<string[]>();
        private readonly string _wweImagesPath = $"{Defaults.ResourceDirectory}/{Defaults.WWEPath}/{Defaults.ImagePath}";
        private readonly string _wweDataPath = $"{Defaults.ResourceDirectory}/{Defaults.WWEPath}/{Defaults.DataPath}";

        public WWECreator(IPDFGenerator generator)
        {
            _generator = generator;
        }

        public List<Wrestler> GetRosterData()
        {
            Workbook sheets = new Workbook($"{_wweDataPath}/{Defaults.WrestlerDataFile}");
            Worksheet worksheet = sheets.Worksheets[1];
            List<Wrestler> rows = new List<Wrestler>();
            Aspose.Cells.Cells cells = worksheet.Cells;
            for (int row = 1; row <= cells.MaxDataRow; row++)
            {
                Wrestler wrestler = new Wrestler
                {
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
            _wrestlers = GetRosterData();
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
            _selectedWrestler1 = _wrestlers.Find(w => w.Name.Equals(wrestler1, StringComparison.OrdinalIgnoreCase));
            _selectedWrestler2 = _wrestlers.Find(w => w.Name.Equals(wrestler2, StringComparison.OrdinalIgnoreCase));
            if (_selectedWrestler1 == null || _selectedWrestler2 == null)
                throw new Exception(Messages.WrestlerNotInRoster);
        }

        public void RenderCard()
        {
            if (_selectedWrestler1 == null || _selectedWrestler2 == null) return;
            // Banner & Header
            _generator.SetBackgroundColor(Color.Black);
            //_generator.CreateWatermark($"{_wweImagesPath}/{Defaults.WWELogoFile}", 0, 0.75F);
            _generator.SetForegroundColor(Color.FloralWhite);
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{Defaults.SurvivorSeriesLogoFile}", 0, (int)_generator.GetPageHeight() - 40, (int)_generator.GetPageWidth(), 40));
            _generator.CreateHeader(new Header
            {
                Font = Fonts.TimesNewRoman,
                Title = "MATCH CARD",
                FontSize = 20,
                Top = 100
            });
            _generator.SetForegroundColor(Color.DarkGoldenrod);
            _generator.CreateTextBox(new Textbox([$"{_selectedWrestler1.Name}"], Fonts.TimesNewRoman, 100, 50, 100, 20, 18));
            _generator.CreateTextBox(new Textbox([$"{_selectedWrestler2.Name}"], Fonts.TimesNewRoman, 380, -18, 100, 20, 18));

            // Wrestler 1
            _generator.SetForegroundColor(Color.FloralWhite);
            _generator.CreateSection(new Textbox(
              [$"Weight: {_selectedWrestler1.Weight} lbs", $"Finisher: {_selectedWrestler1.Finisher}"],
              Fonts.Calibri, 80, 450, 300, 100, 18
            ));
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{_selectedWrestler1.Profile}", 80, 650, 150, 250));
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{_selectedWrestler1.Division}.png", 80, 350, 100, 50));

            // Wrestler 2
            _generator.CreateSection(new Textbox(
             [$"Weight: {_selectedWrestler2.Weight} lbs", $"Finisher: {_selectedWrestler2.Finisher}"],
             Fonts.Calibri, 380, 450, 300, 100, 18
           ));
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{_selectedWrestler2.Profile}", 380, 650, 150, 250));
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{_selectedWrestler2.Division}.png", 380, 350, 100, 50));

            // Mascot & Footer
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{Defaults.WWELogoFile}", 450, 120, 100, 100));
            _generator.CreateImage(new PDFImage($"{_wweImagesPath}/{Defaults.MascotImageFile}", 50, 180, 100, 200));
            _generator.SetForegroundColor(Color.Red);
            _generator.CreateFooter(new Footer
            {
                Font = Fonts.TimesNewRoman,
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
    }
}
