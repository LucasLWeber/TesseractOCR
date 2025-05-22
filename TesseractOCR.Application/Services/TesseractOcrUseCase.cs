using TesseractOCR.Application.Dtos;
using TesseractOCR.Application.Interfaces;

namespace TesseractOCR.Application.Services
{
    public class TesseractOcrUseCase
    {
        private readonly ITesseractOcrService _tesseractOcrService;

        public TesseractOcrUseCase(ITesseractOcrService tesseractOcrService)
        {
            _tesseractOcrService = tesseractOcrService;
        }

        public async Task<TesseractOcrResponse> ExecuteAsync(Stream imageStream)
        {
            return await _tesseractOcrService.ProcessImageAsync(imageStream);
        }
    }
}
