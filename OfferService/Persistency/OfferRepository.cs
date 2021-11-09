using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;

namespace OfferService.Persistency
{
    public class OfferRepository: IOfferRepository
    {
        private readonly OfferContext _context;

        public OfferRepository(OfferContext context)
        {
            _context = context;
        }

        public int AddQuoteToOffer(Guid userId, Quote newQuote)
        {
            Offer currentOffer = _context
                                    .Offers
                                    .Include(x => x.Quotes)
                                    .FirstOrDefault(x => x.UserId == userId && !x.Quotes.Any(q => q.IsApproved));

            if(currentOffer == null)
            {
                currentOffer = new Offer { UserId = userId };
                _context.Offers.Add(currentOffer);
            }

            if (currentOffer.Quotes.All(q => !IsExistingQuote(q, newQuote)))
            {
                currentOffer.Quotes.Add(newQuote);
            }

            _context.SaveChanges();

            return currentOffer.Id;
        }

        private static bool IsExistingQuote(Quote existingQuote, Quote newQuote)
            => existingQuote.BankName == newQuote.BankName
            && Math.Abs(existingQuote.InterestRate - newQuote.InterestRate) < 0.0001
            && Math.Abs(existingQuote.MonthlyPaymentPrecent - newQuote.MonthlyPaymentPrecent) < 0.00001;
    }
}
