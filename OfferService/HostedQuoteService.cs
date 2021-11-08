using Microsoft.Extensions.Hosting;
using OfferService.Integrations;
using OfferService.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OfferService
{
    public class HostedQuoteService: IHostedService
    {
        private readonly IQuoteConsumer _consumer;

        public HostedQuoteService(IQuoteConsumer consumer)
        {
            _consumer = consumer;
        }

        private void OnMessageRecived(BankQuoteMessage data)
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.ListeningForMessages<BankQuoteMessage>(OnMessageRecived);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
