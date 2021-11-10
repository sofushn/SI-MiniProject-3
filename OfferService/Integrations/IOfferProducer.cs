using OfferService.Models;

namespace OfferService.Integrations
{
    public interface IOfferProducer
    {
        void ProduceOfferUpdateMessage(Offer offer);
    }
}