using Aspose.Cells.Rendering;
using Aspose.Cells;
using Aspose.Pdf.Text;
using Aspose.Words.Fonts;
using AsposeTriage.Common;
using AsposeTriage.Models;
using AsposeTriage.Services.Interfaces;
using AsposeTriage.Structures;
using AsposeTriage.Utilities;
using Color = Aspose.Pdf.Color;
using System.Runtime.CompilerServices;

namespace AsposeTriage.Services.Creators
{
    public class CustomCreator
    {
        private readonly IPDFGenerator _generator;
        private readonly IS3Service _s3Service;
        private readonly bool _saveLocal;
        private CustomDAO _customData = new CustomDAO();
        List<string[]> dataRows = new List<string[]>();
        string[] headerRow = [];
        public CustomCreator(IPDFGenerator generator, IS3Service s3Service, bool saveLocal = true)
        {
            _generator = generator;
            _s3Service = s3Service;
            _saveLocal = saveLocal;
        }

        public async Task CreateCustom(CustomDAO custom)
        {
            if (custom != null)
            {
                _customData = custom;
                if (custom.Headers.Any())
                {
                    if (!_saveLocal)
                    {
                        var output = await _s3Service.GetFileFromS3(Defaults.UploadDirectory, custom.Filename);
                        headerRow = SheetManipulator.GetHeadersFromExcelS3(output.Item1, 0).ToArray();
                        dataRows = SheetManipulator.GetRowsFromExcelS3(output.Item1, 0);
                    }
                    else
                    {
                        headerRow = SheetManipulator.GetHeadersFromExcel(custom.Filename, 0).ToArray();
                        dataRows = SheetManipulator.GetRowsFromExcel(custom.Filename, 0);
                    }
                    
                }
                else
                {
                    if (!_saveLocal)
                    {
                        var output = await _s3Service.GetFileFromS3(Defaults.UploadDirectory, custom.Filename);
                        dataRows = SheetManipulator.GetSpecificRowsFromExcelS3(output.Item1, 0, custom.Headers);
                    }
                    else dataRows = SheetManipulator.GetSpecificRowsFromExcel(custom.Filename, 0, custom.Headers);
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
                Font = Fonts.TimesNewRoman,
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
                Font = Fonts.TimesNewRoman,
                Text = _customData.Footer
            });
            #endregion
        }

        public async Task GenerateCustom()
        {
            var filename = $"{_customData.Title}_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.pdf";
            if (!_saveLocal)
            {
                Stream stream = _generator.GeneratePDFStream();
                await _s3Service.LoadStreamInS3(stream, filename, Path.GetExtension(filename), MimeTypes.PDF);
            }
            else _generator.GeneratePDF($"{Defaults.DispatchDirectory}/{filename}");
            _generator.Dispose();
        }
    }
}


