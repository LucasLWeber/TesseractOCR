namespace TesseractOCR.Interfaces;

public interface IQueuePublisher
{
    Task PublishAsync(Stream stream);
}
