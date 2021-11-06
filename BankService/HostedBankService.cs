using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankService.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BankService
{
    public class BankService : IHostedService
    {
        private readonly ILogger<BankService> _logger;
        private readonly KafkaConfig _options;
        private readonly IConsumer<string, string> _consumer;

        public BankService(ILogger<BankService> logger, IOptions<KafkaConfig> options)
        {
            _logger = logger;
            _options = options.Value;

            ConsumerConfig config = new()
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoOffsetStore = false
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service started.");

            _consumer.Subscribe(_options.LoanRequestTopicName);
            TopicPartition partition = new(_options.LoanRequestTopicName, new Partition(0));
            _consumer.Assign(new TopicPartitionOffset(partition, Offset.Beginning));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);
                    var message = result.Message;
                    _logger.LogInformation($"Key: {message.Key}, Value: {message.Value}");
                }
                catch (OperationCanceledException) { }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}