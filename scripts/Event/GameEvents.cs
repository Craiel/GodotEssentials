namespace Craiel.Essentials.Event;

using System;
using Contracts;

public static class GameEvents
{
    private static readonly BaseEventAggregate<IGameEvent> Aggregate = new();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void Send<T>(T eventData)
        where T : IGameEvent
    {
        DoSend(eventData);
    }

    public static void Subscribe<TSpecific>(BaseEventAggregate<IGameEvent>.GameEventAction<TSpecific> actionDelegate, 
        out BaseEventSubscriptionTicket ticket, 
        Func<TSpecific, bool>? filterDelegate = null)
        where TSpecific : IGameEvent
    {
        ticket = DoSubscribe(actionDelegate, filterDelegate);
    }
    
    public static void Unsubscribe(ref BaseEventSubscriptionTicket? ticket)
    {
        DoUnsubscribe(ref ticket);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void DoSend<T>(T eventData)
        where T : IGameEvent
    {
        Aggregate.Send(eventData);
    }
    
    private static BaseEventSubscriptionTicket DoSubscribe<TSpecific>(BaseEventAggregate<IGameEvent>.GameEventAction<TSpecific> actionDelegate, Func<TSpecific, bool>? filterDelegate)
        where TSpecific : IGameEvent
    {
        return Aggregate.Subscribe(actionDelegate, filterDelegate);
    }

    private static void DoUnsubscribe(ref BaseEventSubscriptionTicket? ticket)
    {
        Aggregate.Unsubscribe(ref ticket);
    }
}
