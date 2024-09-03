using FirstApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobService _blobService;

        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(file.FileName, stream);
            }

            return Ok("File uploaded successfully.");
        }

        [HttpGet("download/{blobName}")]
        public async Task<IActionResult> Download(string blobName)
        {
            if (!await _blobService.BlobExistsAsync(blobName))
            {
                return NotFound("Blob not found.");
            }

            var stream = await _blobService.DownloadBlobAsync(blobName);
            return File(stream, "application/octet-stream", blobName);
        }
    }
}
