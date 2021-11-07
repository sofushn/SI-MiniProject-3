using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Models
{
    public record LoanQuoteResponse(string BankName, List<LoanQuote> Quotes, string UserId);
}
