using TesseractOCR.Application.Dtos.Responses;

namespace TesseractOCR.Application.Interfaces
{
    public interface ITesseractOcrDocumentResult
    {
        Task SaveAsync(TesseractOcrResultDocumentResponse document);
        Task<List<TesseractOcrResultDocumentResponse>> GetDocumentsAsync();
    }
}
