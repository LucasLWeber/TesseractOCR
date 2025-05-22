using Microsoft.AspNetCore.Mvc;
using TesseractOCR.Application.Services;
using TesseractOCR.Interfaces;

namespace TesseractOCR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesseractOcrController : ControllerBase
    {
        private readonly GetDocumentsFromMongoUseCase _getDocumentsFromMongoUseCase;

        public TesseractOcrController(GetDocumentsFromMongoUseCase getDocumentsFromMongoUseCase)
        {
            _getDocumentsFromMongoUseCase = getDocumentsFromMongoUseCase;
        }


        [HttpGet]
        public async Task<IActionResult> GetProcessedImagesFromMongo()
        {
            var result = await _getDocumentsFromMongoUseCase.ExecuteAsync();
            return Ok(new { textGenerated = result });
        }

        [HttpPost]
        public async Task<IActionResult> SendToQueue(IFormFile formFile, [FromServices] IQueuePublisher sender)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("Arquivo inválido");
            }
            using var stream = formFile.OpenReadStream();
            await sender.PublishAsync(stream);
            return Ok("Mensagem enviada para fila");
        }
    }
}
