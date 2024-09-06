using Aspose.Pdf.Text;
using AsposeTriage.Common;
using AsposeTriage.Models;
using AsposeTriage.Structures;
using AsposeTriage.Utilities;
using Color = Aspose.Pdf.Color;

namespace AsposeTriage.Services
{
    public class CustomCreator
    {
        private readonly IPDFGenerator _generator;
        private CustomDAO _customData = new CustomDAO();
        List<string[]> dataRows = new List<string[]>();
        string[] headerRow = [];
        public CustomCreator(IPDFGenerator generator)
        {
            _generator = generator;
        }

        public void CreateCustom(CustomDAO custom)
        {
            if(custom != null) {
                this._customData = custom;
                if (custom.Headers.Any())
                {
                    headerRow = SheetManipulator.GetHeadersFromExcel(custom.Filename, 0).ToArray();
                    dataRows = SheetManipulator.GetRowsFromExcel(custom.Filename, 0);
                }
                else
                {
                    dataRows = SheetManipulator.GetSpecificRowsFromExcel(custom.Filename, 0, custom.Headers);
                    headerRow = custom.Headers;
                }
            }
        }

        public void RenderCustom()
        {
            #region header
            _generator.SetBackgroundColor(Color.White);
            _generator.SetForegroundColor(Color.Black);
            _generator.CreateHeader(new Header
            {
                Font = "Times New Roman",
                Title = _customData.Title,
                FontSize = 20,
                Top = 75
            }, FontStyles.Bold);
            #endregion

            #region body
            _generator.SetForegroundColor(Color.Black);
            _generator.CreateTableFromStringRows(new TableData()
            {
                PosX = _customData.RelativeTableX,
                PosY = _customData.RelativeTableY,
                FontSize = _customData.TableFontSize,
                Padding = 5,
                HeaderRows = headerRow,
                DataRows = dataRows,
                AutoFit = true
            }, Color.Transparent, Color.Transparent, true);
            #endregion

            #region footer
            _generator.SetForegroundColor(Color.Black);
            _generator.CreateFooter(new Footer
            {
                Font = "Times New Roman",
                Text = _customData.Footer
            });
            #endregion
        }

        public void GenerateCustom()
        {
            _generator.GeneratePDF($"{Defaults.DispatchDirectory}/{_customData.Title}_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.pdf");
            _generator.Dispose();
        }
    }
}


