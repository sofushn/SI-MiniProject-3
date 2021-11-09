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
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedQuoteService(IQuoteConsumer consumer, IServiceScopeFactory scopeFactory)
        {
            _consumer = consumer;
            _scopeFactory = scopeFactory;
        }

        private void OnMessageRecived(BankQuoteMessageDto data)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IOfferRepository repository = scope.ServiceProvider.GetRequiredService<IOfferRepository>();

            foreach (LoanQuoteDto loanQuote in data.Quotes)
            {
                Quote quote = new()
                {
                    BankName = data.BankName, 
                    InterestRate = loanQuote.InterestRate, 
                    MonthlyPaymentPrecent = loanQuote.MonthlyPaymentPrecent
                };
                int id = repository.AddQuoteToOffer(Guid.Parse(data.UserId), quote);
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
