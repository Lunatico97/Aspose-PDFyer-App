using AsposeTriage.Common;
using AsposeTriage.Services.Creators;
using AsposeTriage.Services.Interfaces;
using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly BillCreator _billCreator;
        public BillController(IPDFGenerator generator) {
            _billCreator = new BillCreator(generator);
        }

        [HttpPost]
        [Route(Routes.GenerateBill)]
        public ActionResult Post(string dataFilename, string location)
        {
            if(location == null)
            {
                return Json(new { success = false, message = Messages.LocationNotProvided });
            }
            try
            {
                if (_billCreator.CheckIfHeadersMatch(dataFilename))
                {
                    _billCreator.CreateBill(dataFilename, location);
                    _billCreator.RenderBill();
                    _billCreator.GenerateBill();
                }
                else return Json(new { success = false, message = $"{Messages.FileWithInvalidHeaderFormat} \n Format: {_billCreator.DisplayRequiredHeaders()}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"{Messages.BillGeneratedFailure} [{ex.Message}]"});
            }
            return Json(new { success = true, message = Messages.BillGeneratedSuccess, date = $"{DateTime.Now.Date.ToString("yyyy-MM-dd")}" });
        }

        [HttpGet]
        [Route(Routes.GetSalesData)]
        public ActionResult Get(string dataFilename)
        {
            return Json(_billCreator.GetSalesData(dataFilename));
        }

        [HttpPost]
        [Route(Routes.ComparePDFCustom)]
        public ActionResult ComparePDFs(IFormFile pdf1, IFormFile pdf2)
        {
            if (pdf1 == null || pdf2 == null)
            {
                return Json(new { success = false, message = Messages.FilesRequiredForComparison });
            }
            try
            {
                List<string[]> checks = DocumentComparator.CompareCustom(pdf1, pdf2);
                _billCreator.GenerateComparisonChecks(checks);
                return Json(new { success = true, message = Messages.CustomComparisonSuccess, downloadFilename = Defaults.CustomCheckPDFFile});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route(Routes.ComparePDFAspose)]
        public ActionResult ComparePDFsAspose(IFormFile pdf1, IFormFile pdf2)
        {
            if (pdf1 == null || pdf2 == null)
            {
                return Json(new { success = false, message = Messages.FilesRequiredForComparison });
            }
            try
            {
                DocumentComparator.CompareAspose(pdf1, pdf2);
                return Json(new { success = true, message = Messages.AsposeComparisonSuccess, downloadFilename = Defaults.AsposeCheckPDFFile});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
