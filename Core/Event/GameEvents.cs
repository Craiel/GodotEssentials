namespace Craiel.Essentials.Event;

using System;
using Contracts;
using EngineCore;

public class GameEvents : IGameModule
{
    private BaseEventAggregate<IGameEvent> aggregate;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Initialize()
    {
        this.aggregate = new BaseEventAggregate<IGameEvent>();
    }

    public void Update(double delta)
    {
    }

    public void Destroy()
    {
    }

    public static void Send<T>(T eventData)
        where T : IGameEvent
    {
        EssentialCore.GameEvents.DoSend(eventData);
    }

    public static void Subscribe<TSpecific>(BaseEventAggregate<IGameEvent>.GameEventAction<TSpecific> actionDelegate, 
        out BaseEventSubscriptionTicket ticket, 
        Func<TSpecific, bool> filterDelegate = null)
        where TSpecific : IGameEvent
    {
        ticket = EssentialCore.GameEvents.DoSubscribe(actionDelegate, filterDelegate);
    }
    
    public static void Unsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        EssentialCore.GameEvents.DoUnsubscribe(ref ticket);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void DoSend<T>(T eventData)
        where T : IGameEvent
    {
        this.aggregate.Send(eventData);
    }
    
    private BaseEventSubscriptionTicket DoSubscribe<TSpecific>(BaseEventAggregate<IGameEvent>.GameEventAction<TSpecific> actionDelegate, Func<TSpecific, bool> filterDelegate)
        where TSpecific : IGameEvent
    {
        return this.aggregate.Subscribe(actionDelegate, filterDelegate);
    }

    private void DoUnsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        this.aggregate.Unsubscribe(ref ticket);
    }
}
