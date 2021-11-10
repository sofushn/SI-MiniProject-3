using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OfferService.Configurations;
using OfferService.Models;

namespace OfferService.Integrations
{
    public class OfferProducer: IOfferProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfig _option;

        public OfferProducer(IOptions<KafkaConfig> options)
        {
            _option = options.Value;

            ProducerConfig config = new()
            {
                BootstrapServers = _option.BootstrapServers
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public void ProduceOfferUpdateMessage(Offer offer)
        {
            Message<string, string> message = new()
            {
                Key = offer.UserId.ToString(),
                Value = JsonSerializer.Serialize(offer)
            };
            _producer.Produce(_option.OfferTopicName, message);
            _producer.Flush();
        }
    }
}
