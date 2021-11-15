using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanUi.Models;
using LoanUi.Services;

namespace LoanUi.Pages
{
    public partial class Index
    {
        public Guid CustomerId { get; set; }
        public LoanOfferDto ActiveOffer { get; set; }
        public Modal modal;

        public Index()
        {
            ActiveOffer = new();
            CustomerId = new("23a0fb75-9191-477e-9ed0-abb4d9f830cb");
        }

        protected override async Task OnInitializedAsync()
        {
            ActiveOffer = await _loanService.FetchOffers(CustomerId);
            _loanService.ActiveOfferUpdated += OnOfferUpdated;
        }


        private void OnOfferUpdated(LoanOfferDto offer)
        {
            if (offer.UserId != CustomerId)
                return;
            
            ActiveOffer = offer;
            InvokeAsync(StateHasChanged);
        }

        void SelectLoanOffer(LoanQuoteDto quote)
            => modal.Open(quote);
        
        void ApproveOffer(LoanQuoteDto quote)
        {
            _loanService.ApproveLoanOffer(ActiveOffer.Id, quote.Id);
            ActiveOffer = null;
            StateHasChanged();
        }

        void RequestLoanClicked()
        {
            _loanService.RequestNewLoan(CustomerId);
        }
    }
}
