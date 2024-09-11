using AsposeTriage.Common;
using AsposeTriage.Models;
using AsposeTriage.Services.Creators;
using AsposeTriage.Services.Interfaces;
using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomController : Controller
    {
        private readonly CustomCreator _customCreator;
        public CustomController(IPDFGenerator generator, IS3Service s3Service)
        {
            _customCreator = new CustomCreator(generator, s3Service, false);
        }

        [HttpGet]
        [Route(Routes.GetCustomDataHeaders)]
        public async Task<ActionResult> Get(string filename)
        {
            if(filename == null)
            {
                return Json(new { success = false, message = Messages.FileNameNotProvided });
            }
            try
            {
                return Json(await _customCreator.GetCustomDataHeaders(filename));
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }
        }

        [HttpPost]
        [Route(Routes.GeneratePDFCustom)]
        public async Task<ActionResult> Post(CustomDAO custom)
        {
            if (custom == null)
            {
                return Json(new { success = false, message = Messages.FileNameNotProvided });
            }
            try
            { 
                await _customCreator.CreateCustom(custom);
                _customCreator.RenderCustom();
                await _customCreator.GenerateCustom();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = Messages.PDFGeneratedSuccess, date = $"{DateTime.Now.Date.ToString("yyyy-MM-dd")}" });
        }
    }
}

