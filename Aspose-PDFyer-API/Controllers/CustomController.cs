using AsposeTriage.Common;
using AsposeTriage.Models;
using AsposeTriage.Services;
using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomController : Controller
    {
        private readonly CustomCreator _customCreator;
        public CustomController(IPDFGenerator generator)
        {
            _customCreator = new CustomCreator(generator);
        }

        [HttpGet]
        [Route(Routes.GetCustomDataHeaders)]
        public ActionResult Get(string filename)
        {
            if(filename == null)
            {
                return Json(new { success = false, message = Messages.FileNameNotProvided });
            }
            try
            {
                return Json(SheetManipulator.GetHeadersFromExcel(filename, 0));
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }
        }

        [HttpPost]
        [Route(Routes.GeneratePDFCustom)]
        public ActionResult Post(CustomDAO custom)
        {
            if (custom == null)
            {
                return Json(new { success = false, message = Messages.FileNameNotProvided });
            }
            try
            { 
                _customCreator.CreateCustom(custom);
                _customCreator.RenderCustom();
                _customCreator.GenerateCustom();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = Messages.PDFGeneratedSuccess, date = $"{DateTime.Now.Date.ToString("yyyy-MM-dd")}" });
        }
    }
}

