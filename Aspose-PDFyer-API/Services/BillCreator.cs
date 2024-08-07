﻿using Aspose.Cells;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using AsposeTriage.Structures;
using AsposeTriage.Models;
using Microsoft.AspNetCore.Mvc;
using AsposeTriage.Utilities;
using AsposeTriage.Common;

namespace AsposeTriage.Services
{
    
    public class BillCreator
    {
        private readonly IPDFGenerator _generator;
        private string? selectedCity;
        private double totalSales = 0;
        private int vatPercent = 13;
        private double grandTotal;
        private List<Sales> _filteredSales = new List<Sales>();
        private List<string> _cities = new List<string>();
        List<string[]> tabularData = new List<string[]>();
        private readonly string[] requiredHeaders = { "ID", "Date", "Region", "City", "Category", "Product", "Quantity", "UnitPrice", "TotalPrice" };
        public BillCreator(IPDFGenerator generator)
        {
            _generator = generator;
        }

        public List<Sales> GetSalesData(string dataFileName)
        {
            List<Sales> _allSales = new List<Sales>();
            Workbook sheets = new Workbook($"{Defaults.UploadDirectory}/{dataFileName}");
            Worksheet worksheet = sheets.Worksheets[0];
            Aspose.Cells.Cells cells = worksheet.Cells;
            for (int row = 1; row <= cells.MaxDataRow; row++)
            {
                _allSales.Add(
                        new Sales
                        {
                            ID = cells[row, 0].StringValue,
                            Date = cells[row, 1].StringValue,
                            Region = cells[row, 2].StringValue,
                            City = cells[row, 3].StringValue,
                            Category = cells[row, 4].StringValue,
                            Product = cells[row, 5].StringValue,
                            Quantity = Convert.ToInt32(cells[row, 6].StringValue),
                            UnitPrice = Convert.ToDouble(cells[row, 7].StringValue),
                            TotalPrice = Convert.ToDouble(cells[row, 8].StringValue)
                        }
                    );
            }
            _cities = _allSales.Select(s => s.City).Distinct().ToList();
            sheets.Dispose();
            return _allSales;
        }

        public string DisplayRequiredHeaders()
        {
            var formattedList = string.Join(", ", requiredHeaders.Select(item => $"'{item}'"));
            return $"[{formattedList}]";
        }

        public bool CheckIfHeadersMatch(string filename)
        {
            List<string> headers = SheetManipulator.GetHeadersFromExcel(filename, 0);
            var headerSet = new HashSet<string>(headers);
            var requiredSet = new HashSet<string>(requiredHeaders);
            return requiredSet.SetEquals(headerSet);
        }

        public void CreateBill(string filename, string location)
        {
            List<Sales> sales = this.GetSalesData(filename);
            selectedCity = _cities.FirstOrDefault(c => c.Equals(location, StringComparison.OrdinalIgnoreCase));
            if (selectedCity == null) throw new Exception(Messages.LocationNotAssociatedToSupplier);
            _filteredSales = sales.Where(s => s.City.Equals(location, StringComparison.OrdinalIgnoreCase))
                                  .GroupBy(t => t.Product)
                                  .Select(g => new Sales
                                  {
                                      Category = g.First().Category,
                                      Product = g.Key,
                                      Quantity = g.Sum(a => a.Quantity),
                                      UnitPrice = g.First().UnitPrice,
                                      TotalPrice = g.Sum(a => a.TotalPrice),

                                  }).OrderBy(r => r.Product).ToList();           
            totalSales = _filteredSales.Sum(f => f.TotalPrice);
            grandTotal = totalSales + ((float)vatPercent/100) * totalSales;
            foreach (Sales s in _filteredSales)
            {
                tabularData.Add(new[]
                {
                    s.Category, s.Product, s.Quantity.ToString(), s.UnitPrice.ToString("C"), s.TotalPrice.ToString("C")
                });
            }
        }

        public void RenderBill()
        {
            #region header
            _generator.SetBackgroundColor(Color.White);
            _generator.SetForegroundColor(Color.Black);
            //_generator.CreateWatermark($"{Defaults.ResourceDirectory}/vegetables.png", 0, 0.75);
            _generator.CreateHeader(new Header
            {
                Font = "Times New Roman", Title = "American Rationwala Pvt. Ltd.",
                FontSize = 20, Top = 75
            }, FontStyles.Bold);
            _generator.SetForegroundColor(Color.Brown);
            _generator.CreateHeader(new Header
            {
                Font = "Times New Roman", Title = "Potato - Potata; We got it covered !",
                FontSize = 12, Top = 20
            }, FontStyles.Italic);
            _generator.CreateImage(new PDFImage($"{Defaults.ResourceDirectory}/{Defaults.BillLogoFile}", 50, 775, 50, 50));
            #endregion

            #region body
            _generator.SetForegroundColor(Color.Blue);
            _generator.CreateSection(new Textbox(
                [$"From: Coventry ", "Khandani Suppliers", "☎️ 247-7923781"], "Calibri", 50, 150, 300, 100
             ));
            _generator.CreateSection(new Textbox(
               [$"To: {selectedCity}", "California Distributors", "☎️ 208-1234567"], "Calibri", 400, 150, 300, 100
            ));
            _generator.SetForegroundColor(Color.Black);
            _generator.CreateHeader(new Header
            {
                Font = "Times New Roman", Title = "INVOICE", FontSize = 16, Top = 20
            }, FontStyles.Bold);
            _generator.CreateTableFromStringRows(new TableData()
            {
                PosX = 50, PosY = 50, FontSize = 12, Padding = 5,
                HeaderRows = ["Category", "Product", "Quantity", "Unit Price", "Total Price"],
                DataRows = tabularData
            }, Color.Transparent, Color.Transparent, false);
            _generator.CreateTextBox(new Textbox([$"Total: $ {totalSales.ToString("F2")}"], "Calibri", 425, 50, 100, 20, 15), FontStyles.Bold, false);
            _generator.SetForegroundColor(Color.Red);
            _generator.CreateTextBox(new Textbox([$"+ VAT: {vatPercent}%"], "Calibri", 450, 5, 100, 20, 15), FontStyles.Bold, true);
            _generator.SetForegroundColor(Color.Green);
            _generator.CreateTextBox(new Textbox([$"Grand Total: $ {grandTotal.ToString("F2")}"], "Calibri", 370, 10, 100, 20, 16), FontStyles.Bold, false);
            #endregion

            #region footer
            _generator.SetForegroundColor(Color.Black);
            _generator.CreateImage(new PDFImage("Input/signature.png", 50, 200, 100, 50));
            _generator.CreateSection(new Textbox(
               [$"Chota Chetan", "Certified Vendor", "chetan@amv.com"], "Calibri", 50, 700, 300, 100
            ));
            _generator.CreateTextBox(new Textbox(
               [$"Date Issued: {DateTime.Now.Date.ToString("yyyy-MM-dd")}"], "Calibri", 400, 0, 300, 100
            ), FontStyles.Bold);
            _generator.CreateFooter(new Footer
            {
                Font = "Times New Roman",
                Text = "Please kindly pay your amount before 30 days of issue to avoid penalty !",
                Link = "https://paypal.com"
            });
            #endregion
        }

        public void GenerateComparisonChecks(List<string[]> checks)
        {
            _generator.CreateTableFromStringRows(new TableData()
            {
                PosX = 50,
                PosY = 50,
                FontSize = 12,
                Padding = 5,
                HeaderRows = ["Check", "Value"],
                DataRows = checks,
                ColumnWidths = "100 400"
            }, Color.Transparent, Color.Transparent, true);
            _generator.GeneratePDF($"{Defaults.DispatchDirectory}/{Defaults.CustomCheckPDFFile}");
            _generator.Dispose();
        }

        public void GenerateBill()
        {
            _generator.GeneratePDF($"{Defaults.DispatchDirectory}/{selectedCity}_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.pdf");
            _generator.Dispose();
        }
    }
}
