namespace TesseractOCR.Application.Interfaces
{
    public interface IQueueConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}
