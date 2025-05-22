using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using TesseractOCR.Application.Dtos.Responses;
using TesseractOCR.Application.Interfaces;
using TesseractOCR.Infrastructure.Config;

namespace TesseractOCR.Infrastructure.Services
{
    public class SqsQueueConsumer : IQueueConsumer
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;
        private readonly ITesseractOcrService _ocrService;
        private readonly ITesseractOcrDocumentResult _mongoRepository;

        public SqsQueueConsumer(
            IAmazonSQS sqsClient,
            IOptions<AwsSqsOptions> options,
            ITesseractOcrService ocrService,
            ITesseractOcrDocumentResult mongoRepository)
        {
            _sqsClient = sqsClient;
            _queueUrl = options.Value.QueueUrl;
            _ocrService = ocrService;
            _mongoRepository = mongoRepository;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 10
                };

                var response = await _sqsClient.ReceiveMessageAsync(request, cancellationToken);

                foreach (var message in response.Messages)
                {
                    try
                    {
                        var input = message.Body;

                        if (input is not null)
                        {
                            var ocrResult = await _ocrService.ProcessImageAsync(input);

                            var document = new TesseractOcrResultDocumentResponse
                            {
                                TesseractOcrResponse = ocrResult,
                                ProcessedAt = DateTime.UtcNow
                            };

                            await _mongoRepository.SaveAsync(document);
                        }

                        await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar imagem: {ex.Message}");
                    }
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
