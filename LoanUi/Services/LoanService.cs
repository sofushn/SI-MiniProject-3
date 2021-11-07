using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using LoanUi.Configurations;
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
    }
}
