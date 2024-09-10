using AsposeTriage.Utilities;
using Microsoft.AspNetCore.Mvc;
using AsposeTriage.Common;
using AsposeTriage.Services.Creators;
using AsposeTriage.Services.Interfaces;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WWEController : Controller
    {
        private readonly WWECreator _wweCreator;
        public WWEController(IPDFGenerator generator, IS3Service s3Service) 
        { 
            _wweCreator = new WWECreator(generator, s3Service, false);
        }

        [HttpPost]
        [Route(Routes.GenerateRoster)]
        public ActionResult Post()
        {
            try
            {  
                return Json(new { success = true, message = Messages.RosterGenerated });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route(Routes.GenerateMatchCard)]
        public async Task<ActionResult> Post(string wrestler1, string wrestler2)
        {
            if (wrestler1 == null || wrestler2 == null)
            {
                return Json(new { success = false, message = Messages.WrestlerNotProvided });
            }
            try
            {
                _wweCreator.CreateRoster();
                _wweCreator.CreateMatchCard(wrestler1, wrestler2);
                _wweCreator.RenderCard();
                var filename = await _wweCreator.GenerateCard();
                return Json(new { success = true, message = Messages.MatchCardGenerated, downloadFilename = filename });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route(Routes.GetWrestlersInfo)]
        public ActionResult Get()
        {
            return Json(SheetManipulator.GetRowsFromExcel(Defaults.WrestlerDataFile, 1));
        }
    }
}
