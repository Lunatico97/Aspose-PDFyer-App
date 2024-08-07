﻿using Aspose.Pdf;
using AsposeTriage.Models;
using AsposeTriage.Services;
using AsposeTriage.Utilities;
using AsposeTriage.Structures;
using Microsoft.AspNetCore.Mvc;
using AsposeTriage.Common;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WWEController : Controller
    {
        private readonly WWECreator _wweCreator;
        public WWEController(IPDFGenerator generator) 
        { 
            _wweCreator = new WWECreator(generator);
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
        public ActionResult Post(string wrestler1, string wrestler2)
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
                var filename = _wweCreator.GenerateCard();
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
