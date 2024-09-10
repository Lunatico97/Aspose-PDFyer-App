using AsposeTriage.Common;
using AsposeTriage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3StorageController : Controller
    {
        private readonly IS3Service _s3Service;

        public S3StorageController(IS3Service s3Service) 
        {
            _s3Service = s3Service;
        }

        [HttpPost(Routes.S3FileUpload)]
        public async Task<IActionResult> Upload(IFormFile dataFile)
        {
            if (dataFile == null || dataFile.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }
            try
            {
                var succeeded = await _s3Service.PutFileInS3(dataFile, Defaults.UploadDirectory);
                return Json(new { success = succeeded, message = $"{Messages.FileUploadSuccess} [{dataFile.FileName}]" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"{Messages.FileUploadFailure} [{ex.Message}]" });
            }
        }

        [HttpGet(Routes.S3FileDownload)]
        public async Task<IActionResult> Download(string fileName, string type)
        {
            try
            {
                var outputStream = await _s3Service.GetFileFromS3(Defaults.DispatchDirectory, fileName);
                return File(outputStream.Item1, outputStream.Item2, fileName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}