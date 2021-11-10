using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfferService.Configurations;
using OfferService.Integrations;
using OfferService.Persistency;

namespace OfferService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddOptions<RabbitMqConfig>().BindConfiguration("OfferService:RabbitMq");
                    collection.AddOptions<KafkaConfig>().BindConfiguration("OfferService:Kafka");
                    collection.AddSingleton<IQuoteConsumer, QuoteConsumer>();
                    collection.AddScoped<IOfferRepository, OfferRepository>();
                    collection.AddHostedService<HostedQuoteService>();
                    
                    string connString = context.Configuration.GetConnectionString("OfferDb");
                    collection.AddDbContext<OfferContext>(options => options.UseSqlite(connString));
                });
    }

    public static class HelperExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            OfferContext db = scope.ServiceProvider.GetRequiredService<OfferContext>();
            db.Database.Migrate();
            return host;
        }
    }
}
