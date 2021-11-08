using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfferService.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OfferService.Integrations
{
    public interface IQuoteConsumer: IDisposable
    {
        void WaitForQuoteFromBank();
    }
    public class QuoteConsumer: IQuoteConsumer
    {
        private readonly ILogger<QuoteConsumer> _logger;
        private readonly RabbitMqConfig _config;
        private readonly IConnection _connection;

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

        public void WaitForQuoteFromBank()
        {
            using IModel channel = _connection.CreateModel();
            channel.QueueDeclare(_config.LoanQuoteQueueName, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                _logger.LogInformation(message);

                //channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(_config.LoanQuoteQueueName, true, consumer);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
