using Microsoft.AspNetCore.Mvc;

namespace WEB_253503_Yarmak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly string _imagePath;
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }
        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest("There is No file");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_imagePath, fileName);

            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{fileName}";

            return Ok(fileUrl);
        }
        
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok("File deleted");
            }
            return NotFound("File not found.");
        }
    }
}
