using MongoDB.Bson;

namespace TesseractOCR.Application.Dtos.Responses
{
    public class TesseractOcrResultDocumentResponse
    {
        public ObjectId Id { get; set; }
        public TesseractOcrResponse TesseractOcrResponse { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
    }
}
