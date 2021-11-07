using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankService.Models;

namespace BankService.Configurations
{
    public class BankServiceConfig
    {
        public string BankName { get; set; }
        public List<LoanQuote> LoanQuotes { get; set; }
    }
}
