using AgendaApp.Server.DTOs;
using AgendaApp.Server.Models;
using AgendaApp.Server.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

public class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly string _queueName;
    private readonly ConnectionFactory _factory;

    public RabbitMQPublisher(IConfiguration config, ILogger<RabbitMQPublisher> logger)
    {
        var rabbitConfig = config.GetSection("RabbitMQ");
        _queueName = rabbitConfig.GetValue<string>("Queue") ?? "contatos";
        _logger = logger;
        _factory = new ConnectionFactory
        {
            HostName = rabbitConfig.GetValue<string>("Host"),
            Port = rabbitConfig.GetValue<int>("Port"),
            UserName = rabbitConfig.GetValue<string>("User"),
            Password = rabbitConfig.GetValue<string>("Password")
        };

    }

    public async Task InitializeAsync()
    {
        var attempts = 0;
        while (true)
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                await _channel.QueueDeclareAsync(_queueName, false, false, false);
                _logger.LogInformation("Conectando ao RabbitMQ em {Host}:{Port}", _factory.HostName, _factory.Port);
                break;
            }
            catch (BrokerUnreachableException ex) when (attempts++ < 5)
            {
                _logger.LogInformation(ex.ToString());
                await Task.Delay(1000 * attempts);
            }
        }
    }

    public async Task PublishEvent(Contato contato, ContatoEventType eventType)
    {
        var message = JsonSerializer.Serialize(new ContatoEventDTO
        {
            Id = contato.Id,
            Nome = contato.Nome,
            Email = contato.Email,
            Telefone = contato.Telefone,
            Evento = eventType,
            Timestamp = DateTime.UtcNow
        });

        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties { Persistent = true };
        await _channel.BasicPublishAsync<BasicProperties>(
            exchange: "",
            routingKey: _queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: CancellationToken.None
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel?.IsOpen == true)
        {
            await _channel.CloseAsync();
        }
        if (_connection?.IsOpen == true)
        {
            await _connection.CloseAsync();
        }

        _channel?.Dispose();
        _connection?.Dispose();
    }
}
