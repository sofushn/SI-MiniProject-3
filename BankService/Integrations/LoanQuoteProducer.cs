using BankService.Configurations;
using BankService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace BankService.Integrations
{
    public class LoanQuoteProducer: ILoanQuoteProducer
    {
        private readonly ILogger<LoanQuoteProducer> _logger;
        private readonly RabbitMqConfig _config;
        private readonly IConnection _connection;

        public LoanQuoteProducer(ILogger<LoanQuoteProducer> logger, IOptions<RabbitMqConfig> options)
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

        public void SendLoanOfferResponse(LoanQuoteResponse quoteResponse)
        {
            using IModel channel = _connection.CreateModel();

            channel.QueueDeclare(_config.OfferQueueName, exclusive: false, autoDelete: false);

            byte[] offer = JsonSerializer.SerializeToUtf8Bytes(quoteResponse);
            channel.BasicPublish("", _config.OfferQueueName, body: offer);
            _logger.LogInformation($"Offer was send for {quoteResponse.UserId}.");
        }


        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
