﻿using Aspose.Cells;
using Aspose.Pdf;
using AsposeTriage.Models;
using AsposeTriage.Structures;
using System.Collections;

namespace AsposeTriage.Utilities
{
    public class SheetManipulator
    {
        private static readonly string _dataDir = "Output";
        public static void GeneratePDFUsingSheet(string inSheetName, string outFilename)
        {
            Workbook sheets = new Workbook($"Uploads/{inSheetName}.xlsx");
            var outputFileName = System.IO.Path.Combine(_dataDir, $"{outFilename}.pdf");
            sheets.Save(outputFileName, Aspose.Cells.SaveFormat.Pdf);
        }

        public static List<string> GetHeadersFromExcel(string sheetName, int sheetNum)
        {
            List<string> headers = new List<string>();
            Workbook sheets = new Workbook($"Uploads/{sheetName}");
            Worksheet worksheet = sheets.Worksheets[sheetNum];
            for(int col=0; col <= worksheet.Cells.MaxDataColumn; col++) 
            {
                headers.Add(worksheet.Cells[0, col].StringValue);
            }
            sheets.Dispose();
            return headers;
        }

        public static List<string[]> GetRowsFromExcel(string sheetName, int sheetNum)
        {
            Workbook sheets = new Workbook($"Uploads/{sheetName}");
            Worksheet worksheet = sheets.Worksheets[sheetNum];
            List<string[]> rows = new List<string[]>();
            Aspose.Cells.Cells cells = worksheet.Cells;
            for (int row = 1; row <= cells.MaxDataRow; row++)
            {
                string[] rowData = new string[cells.MaxDataColumn+1];
                for (int col = 0; col <= cells.MaxDataColumn; col++)
                {
                    rowData[col] = cells[row, col].StringValue;
                }
                rows.Add(rowData);
            }
            sheets.Dispose();
            return rows;
        }

        public static List<string[]> GetSpecificRowsFromExcel(string sheetName, int sheetNum, string[] headers)
        {
            Workbook sheets = new Workbook($"Uploads/{sheetName}");
            Worksheet worksheet = sheets.Worksheets[sheetNum];
            List<string[]> rows = new List<string[]>();
            Aspose.Cells.Cells cells = worksheet.Cells;
            for (int row = 1; row <= cells.MaxDataRow; row++)
            {
                List<string> rowData = new List<string>();
                for (int col = 0; col <= cells.MaxDataColumn; col++)
                {
                    if (headers.Contains(cells[0, col].StringValue))
                    { 
                        rowData.Add(cells[row, col].StringValue);
                    }
                    else
                    {
                        //throw new Exception($"The column header - {header} doesn't exist in the file you provided !");
                        continue;
                    }
                }
                rows.Add(rowData.ToArray());
            }
            sheets.Dispose();
            return rows;
        }
    }
}
