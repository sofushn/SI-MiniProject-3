using System;
using BankService.Configurations;
using BankService.Integrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankService
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostbuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostbuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, collection) =>
            {
                collection.AddOptions<BankServiceConfig>().BindConfiguration("BankService").ValidateDataAnnotations();
                collection.AddOptions<RabbitMqConfig>().BindConfiguration("BankService:RabbitMq");
                collection.AddOptions<KafkaConfig>().BindConfiguration("BankService:Kafka");
                collection.AddScoped<ILoanRequestConsumer, LoanRequestConsumer>();
                collection.AddScoped<ILoanQuoteProducer, LoanQuoteProducer>();
                collection.AddHostedService<BankService>();
            });
    }
}
