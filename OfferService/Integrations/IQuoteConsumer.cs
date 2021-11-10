using System;

namespace OfferService.Integrations
{
    public interface IQuoteConsumer: IDisposable
    {
        void ListeningForMessages<T>(Action<T> callback);
    }
}