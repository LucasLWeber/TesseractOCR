using Amazon.SQS;
using Amazon.SQS.Model;
using TesseractOCR.Infrastructure.Config;
using TesseractOCR.Interfaces;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace TesseractOCR.Infrastructure.Services
{
    public class SqsQueueSender : IQueuePublisher
    {

        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public SqsQueueSender(IAmazonSQS sqsClient, IOptions<AwsSqsOptions> options)
        {
            _sqsClient = sqsClient;
            _queueUrl = options.Value.QueueUrl;
        }

        public async Task PublishAsync(Stream stream)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = await StreamToBase64(stream)
            };

            await _sqsClient.SendMessageAsync(request);

        }

        private async Task<string> StreamToBase64(Stream stream)
        {
            var tempPngFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

            using (var pngFile = await Image.LoadAsync<Rgba32>(stream))
            {
                await pngFile.SaveAsPngAsync(tempPngFile);
            }

            byte[] pngBytes = await File.ReadAllBytesAsync(tempPngFile);

            return Convert.ToBase64String(pngBytes);
        }
    }
}
