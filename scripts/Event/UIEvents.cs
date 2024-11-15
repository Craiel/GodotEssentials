namespace Craiel.Essentials.Event;

using System;

public static class UIEvents
{
    private static readonly BaseEventAggregate<IUIEvent> Aggregate = new();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void Send<T>(T eventData)
        where T : IUIEvent
    {
        DoSend(eventData);
    }

    public static void Subscribe<TSpecific>(BaseEventAggregate<IUIEvent>.GameEventAction<TSpecific> actionDelegate, 
        out BaseEventSubscriptionTicket ticket, 
        Func<TSpecific, bool> filterDelegate = null)
        where TSpecific : IUIEvent
    {
        ticket = DoSubscribe(actionDelegate, filterDelegate);
    }
    
    public static void Unsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        DoUnsubscribe(ref ticket);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void DoSend<T>(T eventData)
        where T : IUIEvent
    {
        Aggregate.Send(eventData);
    }
    
    private static BaseEventSubscriptionTicket DoSubscribe<TSpecific>(BaseEventAggregate<IUIEvent>.GameEventAction<TSpecific> actionDelegate, Func<TSpecific, bool> filterDelegate)
        where TSpecific : IUIEvent
    {
        return Aggregate.Subscribe(actionDelegate, filterDelegate);
    }

    private static void DoUnsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        Aggregate.Unsubscribe(ref ticket);
    }
}