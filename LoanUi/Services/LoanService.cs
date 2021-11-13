using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LoanUi.Configurations;
using LoanUi.Models;
using Microsoft.Extensions.Options;

namespace LoanUi.Services
{
    public class LoanService: ILoanService
    {
        private readonly KafkaConfig _options;

        public LoanService(IOptions<KafkaConfig> options)
        {
            _options = options.Value;
        }

        public event Action<LoanOfferDto> ActiveOfferUpdated;

        public void RequestNewLoan(Guid customerId)
        {
            ProducerConfig config = new()
            {
                BootstrapServers = _options.BootstrapServers
            };

            using IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();
            
            string topic = _options.LoanRequestTopicName;
            Message<string, string> message = new()
            {
                Key = customerId.ToString(),
                Value = "New loan is requested"
            };
            producer.Produce(topic, message);
            producer.Flush();
        }

        public void InvokeOfferEvent(LoanOfferDto loanOffer)
            => ActiveOfferUpdated?.Invoke(loanOffer);

        /// <exception cref="ConsumeException"/>
        public Task<LoanOfferDto> FetchOffers(Guid userId)
        {
            ConsumerConfig config = new()
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true,
                EnablePartitionEof = true
            };
            var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(_options.OfferTopicName);
            TopicPartition topicPartition = new(_options.OfferTopicName, new Partition(0));
            consumer.Assign(new TopicPartitionOffset(topicPartition, Offset.Beginning));

            return Task.Run(() => Consume(consumer, userId));
        }

        private static LoanOfferDto Consume(IConsumer<string, string> consumer, Guid userId)
        {
            Message<string, string> message = null;
            ConsumeResult<string, string> result;
            do
            {
                result = consumer.Consume(TimeSpan.FromSeconds(10));
                if (result.Message != null && result.Message.Key == userId.ToString())
                    message = result.Message;

            } while (!result.IsPartitionEOF);
            return message is null
                ? null
                : JsonSerializer.Deserialize<LoanOfferDto>(message.Value);
        }
    }
}
