using Xunit;
using Moq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using TesseractOCR.Infrastructure.Config;
using TesseractOCR.Infrastructure.Services;
using System.Text;
using FluentAssertions;

namespace TesseractOCR.Tests.Infrastructure
{
    public class SqsQueueSenderTests
    {
        [Fact]
        public async Task PublishAsync_ShouldSendMessageToSqs()
        {
            // Arrange
            var mockSqsClient = new Mock<IAmazonSQS>();
            var options = Options.Create(new AwsSqsOptions
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/my-queue"
            });

            var sender = new SqsQueueSender(mockSqsClient.Object, options);

            var imagePath = Path.Combine("Data", "A revolução dos bichos.webp");
            await using var imageStream = File.OpenRead(imagePath);

            mockSqsClient
                .Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(new SendMessageResponse());

            // Act
            await sender.PublishAsync(imageStream);

            // Assert
            mockSqsClient.Verify(sqs => sqs.SendMessageAsync(
                It.Is<SendMessageRequest>(req =>
                    req.QueueUrl == options.Value.QueueUrl &&
                    !string.IsNullOrWhiteSpace(req.MessageBody)
                ),
                default
            ), Times.Once);
        }
    }
}
