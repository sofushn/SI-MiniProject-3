using System;
using BankService.Models;

namespace BankService.Integrations
{
    public interface ILoanQuoteProducer: IDisposable
    {
        void SendLoanOfferResponse(LoanQuoteResponse quoteResponse);
    }
}