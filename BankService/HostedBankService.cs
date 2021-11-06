using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BankService
{
    public class BankService : IHostedService
    {
        private readonly ILogger<BankService> _logger;
        private readonly IConsumer<string, string> _consumer;

        public BankService(ILogger<BankService> logger)
        {
            _logger = logger;
            
            ConsumerConfig config = new()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "bank2-consumer",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoOffsetStore = false
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service started.");

            _consumer.Subscribe("Loan.Request");
            TopicPartition partition = new("Loan.Request", new Partition(0));
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