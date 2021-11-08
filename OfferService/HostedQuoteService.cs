using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OfferService.Integrations;

namespace OfferService
{
    public class HostedQuoteService: IHostedService
    {
        private readonly IQuoteConsumer _consumer;

        public HostedQuoteService(IQuoteConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => _consumer.WaitForQuoteFromBank(), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
