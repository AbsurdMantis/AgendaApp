using AgendaApp.Server.Models;

namespace AgendaApp.Server.Services
{
    public interface IRabbitMQPublisher
    {
        Task InitializeAsync();
        Task PublishEvent(Contato contato, ContatoEventType eventType);
    }
}
