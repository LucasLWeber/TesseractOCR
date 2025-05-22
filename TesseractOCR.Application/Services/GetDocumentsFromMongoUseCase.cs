using TesseractOCR.Application.Dtos.Responses;
using TesseractOCR.Application.Interfaces;

namespace TesseractOCR.Application.Services
{
    public class GetDocumentsFromMongoUseCase
    {
        private readonly ITesseractOcrDocumentResult _repository;

        public GetDocumentsFromMongoUseCase(ITesseractOcrDocumentResult repository)
        {
            _repository = repository;
        }

        public async Task<List<TesseractOcrResultDocumentResponse>> ExecuteAsync()
        {
            return await _repository.GetDocumentsAsync();
        }
    }
}
