using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LoanUi.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LoanUi.Services
{
    public class HostedOfferService: BackgroundService
    {
        private readonly KafkaConfig _options;
        private readonly IConsumer<string, string> _consumer;

        public HostedOfferService(IOptions<KafkaConfig> options)
        {
            _options = options.Value;

            ConsumerConfig config = new()
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();

            _consumer.Subscribe(_options.OfferTopicName);
            TopicPartition topicPartition = new(_options.OfferTopicName, new Partition(0));
            _consumer.Assign(new TopicPartitionOffset(topicPartition, Offset.Beginning));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                var message = result.Message;
            }

            return Task.CompletedTask;
        }
    }
}
