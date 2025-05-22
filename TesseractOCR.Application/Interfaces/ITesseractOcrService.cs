using TesseractOCR.Application.Dtos;

namespace TesseractOCR.Application.Interfaces
{
    public interface ITesseractOcrService
    {
        Task<TesseractOcrResponse> ProcessImageAsync(Stream imageStream);
    }
}
