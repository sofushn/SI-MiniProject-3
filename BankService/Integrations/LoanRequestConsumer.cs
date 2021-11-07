using System.Threading;
using BankService.Configurations;
using BankService.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BankService.Integrations
{
    public class LoanRequestConsumer: ILoanRequestConsumer
    {
        private readonly ILogger<LoanRequestConsumer> _logger;
        private readonly IConsumer<string, string> _consumer;

        public LoanRequestConsumer(ILogger<LoanRequestConsumer> logger, IOptions<KafkaConfig> options)
        {
            _logger = logger;

            KafkaConfig option = options.Value;

            ConsumerConfig config = new()
            {
                BootstrapServers = option.BootstrapServers,
                GroupId = option.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();

            _consumer.Subscribe(option.LoanRequestTopicName);
            TopicPartition topicPartition = new(option.LoanRequestTopicName, new Partition(0));
            _consumer.Assign(new TopicPartitionOffset(topicPartition, Offset.Beginning));
        }

        public LoanRequest WaitForRequest(CancellationToken ct)
        {
            var result = _consumer.Consume(ct);
            var message = result.Message;
            _logger.LogInformation($"Recived loan request: Key={message.Key}, Value={message.Value}");

            return new LoanRequest(message.Key);
        }

        public void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
        }
    }
}
