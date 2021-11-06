using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace LoanUi.Services
{
    public class LoanService: ILoanService
    {
        public void RequestNewLoan(Guid customerId)
        {
            ProducerConfig config = new()
            {
                BootstrapServers = "localhost:9092"
            };

            using IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();
            
            string topic = "Loan.Request";
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
