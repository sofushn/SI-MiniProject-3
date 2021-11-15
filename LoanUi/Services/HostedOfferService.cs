using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LoanUi.Configurations;
using LoanUi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LoanUi.Services
{
    public class HostedOfferService: IHostedService, IDisposable
    {
        private readonly ILoanService _loanService;
        private readonly KafkaConfig _options;
        private readonly IConsumer<string, string> _consumer;
        private Timer _timer;

        public HostedOfferService(IOptions<KafkaConfig> options, ILoanService loanService)
        {
            _loanService = loanService;
            _options = options.Value;

            ConsumerConfig config = new()
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            
            
        }

        private void Consume(object ct)
        {
            ConsumeResult<string, string> result = _consumer.Consume((CancellationToken)ct);
            Message<string, string> message = result.Message;
            LoanOfferDto offer = JsonSerializer.Deserialize<LoanOfferDto>(message.Value);
            if(offer != null)
                _loanService.InvokeOfferEvent(offer);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_options.OfferTopicName);
            TopicPartition topicPartition = new(_options.OfferTopicName, new Partition(0));
            _consumer.Assign(new TopicPartitionOffset(topicPartition, Offset.Beginning));

            _timer = new Timer(Consume, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
