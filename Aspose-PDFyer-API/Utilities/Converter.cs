using Aspose.Cells;
using Aspose.Pdf.Text;
using Aspose.Words;
using Aspose.Words.Pdf2Word.FixedFormats;
using System.Text.RegularExpressions;

namespace AsposeTriage.Utilities
{
    public static class Converter
    {
        public static byte[] ConvertWordToPdf(IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Aspose.Words.Document doc = new Aspose.Words.Document(ms);

                using (MemoryStream outStream = new MemoryStream())
                {
                    doc.Save(outStream, Aspose.Words.SaveFormat.Pdf);
                    return outStream.ToArray();
                }
            }
        }

        public static byte[] ConvertPdfToWord(IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document(ms);

                using (MemoryStream outStream = new MemoryStream())
                {
                    doc.Save(outStream, Aspose.Pdf.SaveFormat.DocX);
                    return outStream.ToArray();
                }
            }
        }

        public static byte[] ConvertExcelOrCsvToPdf(IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Workbook workbook = new Workbook(ms);

                using (MemoryStream outStream = new MemoryStream())
                {
                    workbook.Save(outStream, Aspose.Cells.SaveFormat.Pdf);
                    return outStream.ToArray();
                }
            }
        }

        public static byte[] ConvertExcelOrCsvToWord(IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Workbook workbook = new Workbook(ms);

                using (MemoryStream outStream = new MemoryStream())
                {
                    workbook.Save(outStream, Aspose.Cells.SaveFormat.Docx);
                    return outStream.ToArray();
                }
            }
        }

        public static byte[] FindAndReplaceInPdf(IFormFile formFile, string searchText, string replaceText, bool exactReplacement = true)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document(ms);
                TextFragmentAbsorber textFragmentAbsorber;
                string pattern = @"\b" + Regex.Escape(searchText) + @"\b";
                if(!exactReplacement)
                {
                    textFragmentAbsorber = new TextFragmentAbsorber(searchText);
                }
                else
                {
                    textFragmentAbsorber = new TextFragmentAbsorber(pattern)
                    {
                        TextSearchOptions = new TextSearchOptions(true)
                    };
                }
                doc.Pages.Accept(textFragmentAbsorber);

                foreach (TextFragment textFragment in textFragmentAbsorber.TextFragments)
                {
                    textFragment.Text = replaceText;
                }

                using (MemoryStream outStream = new MemoryStream())
                {
                    doc.Save(outStream, Aspose.Pdf.SaveFormat.Pdf);
                    return outStream.ToArray();
                }
            }
        }
    }
}
