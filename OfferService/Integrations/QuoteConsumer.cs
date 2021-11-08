using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfferService.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace OfferService.Integrations
{
    public interface IQuoteConsumer: IDisposable
    {
        void ListeningForMessages<T>(Action<T> callback);
    }
    public class QuoteConsumer: IQuoteConsumer
    {
        private readonly ILogger<QuoteConsumer> _logger;
        private readonly RabbitMqConfig _config;
        private readonly IConnection _connection;
        private IModel _channel;

        public QuoteConsumer(ILogger<QuoteConsumer> logger, IOptions<RabbitMqConfig> options)
        {
            _logger = logger;
            _config = options.Value;

            ConnectionFactory factory = new()
            {
                UserName = _config.Username,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                HostName = _config.HostName,
                Port = _config.Port
            };

            if (_config.ClientProvidedName != null)
                factory.ClientProvidedName = _config.ClientProvidedName;

            _connection = factory.CreateConnection();
        }

        public void ListeningForMessages<T>(Action<T> callback)
        {
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_config.LoanQuoteQueueName, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                
                string message = Encoding.UTF8.GetString(body);
                _logger.LogInformation(message);

                T messageObject = JsonSerializer.Deserialize<T>(message);

                callback(messageObject);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(consumer, _config.LoanQuoteQueueName);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
