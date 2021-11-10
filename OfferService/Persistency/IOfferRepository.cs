using System;
using OfferService.Models;

namespace OfferService.Persistency
{
    public interface IOfferRepository
    {
        int AddQuoteToOffer(Guid userId, Quote newQuote);
        Offer GetActiveOffer(Guid userId);
    }
}