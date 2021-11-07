using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanUi.Configurations
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string LoanRequestTopicName { get; set; }
    }
}
