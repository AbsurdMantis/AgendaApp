namespace AgendaApp.Server.Services
{
    public class PublisherInitializer : IHostedService
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly ILogger<PublisherInitializer> _logger;

        public PublisherInitializer(IRabbitMQPublisher publisher,
                                    ILogger<PublisherInitializer> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _publisher.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
