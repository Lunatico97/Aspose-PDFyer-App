using AsposeTriage.Common;
using AsposeTriage.Models;
using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFController : Controller
    {
        public PDFController()
        {}

        [HttpPost]
        [Route(Routes.ParseTableFromPDF)]
        public ActionResult Get(IFormFile file)
        {
            List<string[]> result;
            if(file == null) return Json(new { success = false, message = Messages.FileRequired });
            try
            {
                result = PDFManipulator.ParseTableFromPDF(file);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = $"{Messages.PDFParseFailure} [{exception.Message}]" });
            }
            return Json(result);
        }

        [HttpPost]
        [Route(Routes.GeneratePDF)]
        public ActionResult Post(PDFContent pdf)
        {
            try
            {
                PDFManipulator.Generate(pdf.InputHeader, pdf.InputContent, pdf.Filename);
            }
            catch(Exception exception)
            {
                return Json(new { success = false, message = $"{Messages.PDFGeneratedFailure} [{exception.Message}]" });
            }
            return Json( new { success = true, message = Messages.PDFGeneratedSuccess });
        }

        [HttpPost]
        [Route(Routes.GeneratePDFUsingXML)]
        public ActionResult Post(string inXML, string outFilename)
        {
            try
            {
                PDFManipulator.GenerateUsingXML(inXML, outFilename);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = $"{Messages.PDFGeneratedFailure} [{exception.Message}]" });
            }
            return Json(new { success = true, message = Messages.PDFGeneratedSuccess });
        }
    }
}
