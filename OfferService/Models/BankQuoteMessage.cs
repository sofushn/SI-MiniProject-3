using System.Collections.Generic;

namespace OfferService.Models
{
    public record BankQuoteMessage(string BankName, List<LoanQuote> Quotes, string UserId);
}
