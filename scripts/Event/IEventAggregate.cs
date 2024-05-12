namespace Craiel.Essentials.Event;

public interface IEventAggregate
{
    void Unsubscribe(ref BaseEventSubscriptionTicket? ticket);
}