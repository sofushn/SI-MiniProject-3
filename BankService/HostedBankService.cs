using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankService.Configurations;
using BankService.Integrations;
using BankService.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BankService
{
    public class BankService : BackgroundService
    {
        private readonly ILogger<BankService> _logger;
        private readonly ILoanRequestConsumer _requestConsumer;
        private readonly ILoanQuoteProducer _quoteProducer;
        private readonly BankServiceConfig _options;

        public BankService(ILogger<BankService> logger, ILoanRequestConsumer requestConsumer, ILoanQuoteProducer quoteProducer, IOptions<BankServiceConfig> options)
        {
            _logger = logger;
            _requestConsumer = requestConsumer;
            _quoteProducer = quoteProducer;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started.");
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        LoanRequest loanRequest = _requestConsumer.WaitForRequest(stoppingToken);

                        LoanQuoteResponse response = new(_options.BankName, _options.LoanQuotes, loanRequest.UserId);
                        _quoteProducer.SendLoanOfferResponse(response);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Cancellation was requested.");
                    }
                }
            }, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _requestConsumer.Dispose();
            _quoteProducer.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}