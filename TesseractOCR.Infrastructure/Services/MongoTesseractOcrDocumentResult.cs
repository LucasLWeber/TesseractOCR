using MongoDB.Driver;
using TesseractOCR.Application.Dtos.Responses;
using TesseractOCR.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TesseractOCR.Infrastructure.Services
{
    public class MongoTesseractOcrDocumentResult : ITesseractOcrDocumentResult
    {
        private readonly IMongoCollection<TesseractOcrResultDocumentResponse> _collection;
        public MongoTesseractOcrDocumentResult(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDb:Database"]);
            _collection = database.GetCollection<TesseractOcrResultDocumentResponse>("ocrResults");
        }

        public async Task SaveAsync(TesseractOcrResultDocumentResponse document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task<List<TesseractOcrResultDocumentResponse>> GetDocumentsAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
