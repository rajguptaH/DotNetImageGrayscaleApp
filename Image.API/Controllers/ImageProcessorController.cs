using Image.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace Image.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        private readonly ImageProcessor _processor;

        public ImageController(ImageProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost("grayscale")]
        public async Task<IActionResult> ConvertToGrayscale()
        {
            using var stream = new MemoryStream();
            await Request.Body.CopyToAsync(stream);
            stream.Position = 0;

            using var inputBitmap = new Bitmap(stream);
            using var outputBitmap = _processor.ConvertToGrayscale(inputBitmap); 

            using var ms = new MemoryStream();
            outputBitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            return File(ms.ToArray(), "image/png");
        }

    }
}
