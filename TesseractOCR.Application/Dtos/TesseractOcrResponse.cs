
namespace TesseractOCR.Application.Dtos
{
    public class TesseractOcrResponse
    {
        public string FullText { get; set; } = string.Empty;
        public float MeanConfidence { get; set; }
        public List<TesseractOcrWordDto> Words { get; set; } = new();

    }

    public class TesseractOcrWordDto
    {
        public string Text { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public BoundingBoxDto BoundingBox { get; set; } = new();

    }

    public class BoundingBoxDto
    {
        public int X1 { get; set; } 
        public int Y1 { get; set; } 
        public int X2 { get; set; } 
        public int Y2 { get; set; } 
    }
}
