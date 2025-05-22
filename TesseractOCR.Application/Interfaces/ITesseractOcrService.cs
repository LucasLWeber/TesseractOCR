using TesseractOCR.Application.Dtos.Responses;

namespace TesseractOCR.Application.Interfaces;

public interface ITesseractOcrService
{
    Task<TesseractOcrResponse> ProcessImageAsync(string base64File);
}
