using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientProvidedName { get; set; }


        public string OfferQueueName { get; set; }
    }
}
