using System;
using BankService.Configurations;
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
                collection.AddOptions<KafkaConfig>().BindConfiguration("BankService:Kafka");
                collection.AddHostedService<BankService>();
            });
    }
}
