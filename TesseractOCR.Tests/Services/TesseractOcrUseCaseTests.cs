//using Moq;
//using TesseractOCR.Application.Interfaces;
//using TesseractOCR.Application.Services;
//using Xunit;
//using FluentAssertions;
//using TesseractOCR.Application.Dtos.Responses;

//namespace TesseractOCR.Tests.Services
//{
//    public class TesseractOcrUseCaseTests
//    {
//        [Fact]
//        public async Task ExecuteAsync_ShouldReturnValidResponse_WhenImageIsValid()
//        {
//            var imagePath = Path.Combine("Data", "A revolução dos bichos.webp");
//            await using var imageStream = File.OpenRead(imagePath);

//            var mockTesseractOcrService = new Mock<ITesseractOcrService>();

//            mockTesseractOcrService
//                .Setup(s => s.ProcessImageAsync(It.IsAny<Stream>()))
//                .ReturnsAsync(new TesseractOcrResponse
//                {
//                    FullText = "A revolução dos bichos",
//                    MeanConfidence = 0.90f,
//                    Words = new List<TesseractOcrWordDto>
//                    {
//                        new TesseractOcrWordDto
//                        {
//                            Text = "A",
//                            Confidence = 93.2f,
//                            BoundingBox = new BoundingBoxDto
//                            {
//                                X1 = 64,
//                                Y1 = 137,
//                                X2 = 84,
//                                Y2 = 158
//                            }
//                        }
//                    }
//                });

//            var useCase = new TesseractOcrUseCase(mockTesseractOcrService.Object);

//            var result = await useCase.ExecuteAsync(imageStream);

//            result.FullText.Should().Contain("A revolução dos bichos");
//            result.MeanConfidence.Should().BeGreaterThan(0.80f);
//        }
//    }
//}
