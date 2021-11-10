using System;
using Microsoft.Extensions.Hosting;
using OfferService.Integrations;
using OfferService.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OfferService.Persistency;

namespace OfferService
{
    public class HostedQuoteService: IHostedService
    {
        private readonly IQuoteConsumer _consumer;
        private readonly IOfferProducer _offerProducer;
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedQuoteService(IQuoteConsumer consumer, IOfferProducer offerProducer, IServiceScopeFactory scopeFactory)
        {
            _consumer = consumer;
            _offerProducer = offerProducer;
            _scopeFactory = scopeFactory;
        }

        private void OnMessageRecived(BankQuoteMessageDto data)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IOfferRepository repository = scope.ServiceProvider.GetRequiredService<IOfferRepository>();

            Guid userId = Guid.Parse(data.UserId);
            bool isOfferUpdated = false;
            foreach (LoanQuoteDto loanQuote in data.Quotes)
            {
                Quote quote = new()
                {
                    BankName = data.BankName, 
                    InterestRate = loanQuote.InterestRate, 
                    MonthlyPaymentPrecent = loanQuote.MonthlyPaymentPrecent
                };
                isOfferUpdated |= repository.AddQuoteToOffer(userId, quote);
            }

            if(isOfferUpdated)
            {
                Offer activeOffer = repository.GetActiveOffer(userId);
                _offerProducer.ProduceOfferUpdateMessage(activeOffer);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.ListeningForMessages<BankQuoteMessageDto>(OnMessageRecived);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
