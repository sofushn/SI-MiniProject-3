using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferService.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientProvidedName { get; set; }


        public string LoanQuoteQueueName { get; set; }
    }
}
