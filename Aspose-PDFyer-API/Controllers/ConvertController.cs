using AsposeTriage.Common;
using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        [HttpPost(Routes.Word2PDF)]
        public IActionResult ConvertWordToPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Converter.ConvertWordToPdf(file);
            return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(file.FileName)}.pdf");
        }

        [HttpPost(Routes.PDF2Word)]
        public IActionResult ConvertPdfToWord(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var docxBytes = Converter.ConvertPdfToWord(file);
            return File(docxBytes, MimeTypes.DOCX, $"{Path.GetFileNameWithoutExtension(file.FileName)}.docx");
        }

        [HttpPost(Routes.Excel2Word)]
        public IActionResult ConvertExcelOrCsvToWord(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Converter.ConvertExcelOrCsvToWord(file);
            return File(pdfBytes, MimeTypes.DOCX, $"{Path.GetFileNameWithoutExtension(file.FileName)}.docx");
        }

        [HttpPost(Routes.Excel2PDF)]
        public IActionResult ConvertExcelOrCsvToPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Converter.ConvertExcelOrCsvToPdf(file);
            return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(file.FileName)}.pdf");
        }

        [HttpPost(Routes.FindAndReplace)]
        public IActionResult FindAndReplace(IFormFile file, string findText, string replaceText, bool exactReplacementFlag)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Converter.FindAndReplaceInPdf(file, findText, replaceText, exactReplacementFlag);
            return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(file.FileName)}.pdf");
        }

        [HttpPost(Routes.Encrypt)]
        public IActionResult EncryptPdf(IFormFile file, string ownerPwd, string userPwd)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Optimizer.EncryptPDF(file, ownerPwd, userPwd);
            return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(file.FileName)}_Encrypted.pdf");
        }

        [HttpPost(Routes.Compress)]
        public IActionResult CompressPdf(IFormFile file, int imageQuality)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }

            var pdfBytes = Optimizer.CompressPDF(file, imageQuality);
            return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(file.FileName)}_Compressed.pdf");
        }

        [HttpPost(Routes.Merge)]
        public IActionResult MergePdf(IFormFile[] files)
        {
            if (files == null || files.Count() < 2)
            {
                return BadRequest(Messages.MoreThanOneFileToMerge);
            }
            else if(files.Count() > 4)
            {
                return BadRequest(Messages.GetLicenseToMergeMorePages);
            }
            else
            {
                var pdfBytes = DocumentComparator.MergeDocuments(files);
                return File(pdfBytes, MimeTypes.PDF, $"{Path.GetFileNameWithoutExtension(files[0].FileName)}_Merged.pdf");
            }
        }
    }
}
