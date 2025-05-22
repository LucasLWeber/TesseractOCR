using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TesseractOCR.Application.Interfaces;

namespace TesseractOCR.Infrastructure.Services
{
    public class SqsQueueConsumerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SqsQueueConsumerWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IQueueConsumer>();

            await consumer.ConsumeAsync(stoppingToken);
        }
    }
}
