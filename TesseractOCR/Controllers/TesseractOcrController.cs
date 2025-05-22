using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesseractOCR.Application.Services;

namespace TesseractOCR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesseractOcrController : ControllerBase
    {
        private readonly TesseractOcrUseCase _tesseractOcrUseCase;

        public TesseractOcrController(TesseractOcrUseCase tesseractOcrUseCase)
        {
            _tesseractOcrUseCase = tesseractOcrUseCase;
        }


        [HttpPost]
        public async Task<IActionResult> processImage(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("Arquivo inválido");
            }

            using var stream = formFile.OpenReadStream();
            var result = await _tesseractOcrUseCase.ExecuteAsync(stream);
            return Ok(new { textGenerated = result });
        }
    }
}
