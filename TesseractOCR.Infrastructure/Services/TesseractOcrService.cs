using Tesseract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TesseractOCR.Application.Interfaces;
using TesseractOCR.Application.Dtos.Responses;

namespace TesseractOCR.Infrastructure.Services
{
    public class TesseractOcrService : ITesseractOcrService
    {
        public async Task<TesseractOcrResponse> ProcessImageAsync(string base64Png)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Png);

            using var memoryStream = new MemoryStream(imageBytes);

            var tempPngFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

            using (var image = await Image.LoadAsync<Rgba32>(memoryStream))
            {
                await image.SaveAsPngAsync(tempPngFile);
            }

            var tessDataPath = Path.Combine(Path.GetDirectoryName(typeof(TesseractOcrService).Assembly.Location)!, "tessdata");

            using var engine = new TesseractEngine(tessDataPath, "por", EngineMode.Default);
            using var pix = Pix.LoadFromFile(tempPngFile);
            using var page = engine.Process(pix);

            var response = new TesseractOcrResponse
            {
                FullText = page.GetText(),
                MeanConfidence = page.GetMeanConfidence(),
                Words = GetWordsListFromPage(page)
            };

            return response;
        }

        private List<TesseractOcrWordDto> GetWordsListFromPage(Page page)
        {
            using var iterator = page.GetIterator();
            iterator.Begin();

            var list = new List<TesseractOcrWordDto>();

            do
            {
                string word = iterator.GetText(PageIteratorLevel.Word);
                float confidence = iterator.GetConfidence(PageIteratorLevel.Word);

                if (!string.IsNullOrWhiteSpace(word) && iterator.TryGetBoundingBox(PageIteratorLevel.Word, out var rect))
                {
                    list.Add(new TesseractOcrWordDto
                    {
                        Text = word,
                        Confidence = confidence,
                        BoundingBox = new BoundingBoxDto
                        {
                            X1 = rect.X1,
                            Y1 = rect.Y1,
                            X2 = rect.X2,
                            Y2 = rect.Y2
                        }
                    });
                }
            } while (iterator.Next(PageIteratorLevel.Word));

            return list;
        }
    }
}
