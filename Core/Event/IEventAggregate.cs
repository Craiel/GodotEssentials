﻿namespace Craiel.Essentials.Runtime.Event;

public interface IEventAggregate
{
    void Unsubscribe(ref BaseEventSubscriptionTicket ticket);
}