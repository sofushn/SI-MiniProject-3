using System.Collections.Generic;

namespace OfferService.Models
{
    public record BankQuoteMessageDto(string BankName, List<LoanQuoteDto> Quotes, string UserId);
}
