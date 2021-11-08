using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfferService.Configurations;
using OfferService.Integrations;

namespace OfferService
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
                    collection.AddOptions<RabbitMqConfig>().BindConfiguration("OfferService:RabbitMq");
                    collection.AddScoped<IQuoteConsumer, QuoteConsumer>();
                    collection.AddHostedService<HostedQuoteService>();
                });
    }
}
