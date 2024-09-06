using AsposeTriage.Common;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTriage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {

        [HttpPost(Routes.FileUpload)]
        public async Task<IActionResult> Upload(IFormFile dataFile)
        {
            if (dataFile == null || dataFile.Length == 0)
            {
                return BadRequest(Messages.FileRequired);
            }
            try
            {
                using (var stream = new FileStream($"{Defaults.UploadDirectory}/{dataFile.FileName}", FileMode.OpenOrCreate))
                {
                    await dataFile.CopyToAsync(stream);
                }
                return Json(new { success = true, message = $"{Messages.FileUploadSuccess} [{dataFile.FileName}]"});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"{Messages.FileUploadFailure} [{ex.Message}]"});
            }
        }

        [HttpGet(Routes.FileDownload)]
        public IActionResult Download(string fileName, string type)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), Defaults.DispatchDirectory, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(Messages.FileNotFound);
                }
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                string mimeType = $"application/{type}";
                return File(fileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}