using System;
using System.Threading.Tasks;
using LoanUi.Models;

namespace LoanUi.Services
{
    public interface ILoanService
    {
        event Action<LoanOfferDto> ActiveOfferUpdated;
        void RequestNewLoan(Guid customerId);
        void InvokeOfferEvent(LoanOfferDto loanOffer);
        Task<LoanOfferDto> FetchOffers(Guid userId);
    }
}