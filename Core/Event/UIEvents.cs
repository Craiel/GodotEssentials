namespace Craiel.Essentials.Event;

using System;
using EngineCore;

public class UIEvents : IGameModule
{
    private BaseEventAggregate<IUIEvent> aggregate;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Initialize()
    {
        this.aggregate = new BaseEventAggregate<IUIEvent>();
    }

    public void Update(double delta)
    {
    }

    public void Destroy()
    {
    }

    public static void Send<T>(T eventData)
        where T : IUIEvent
    {
        EssentialCore.UIEvents.DoSend(eventData);
    }

    public static void Subscribe<TSpecific>(BaseEventAggregate<IUIEvent>.GameEventAction<TSpecific> actionDelegate, 
        out BaseEventSubscriptionTicket ticket, 
        Func<TSpecific, bool> filterDelegate = null)
        where TSpecific : IUIEvent
    {
        ticket = EssentialCore.UIEvents.DoSubscribe(actionDelegate, filterDelegate);
    }
    
    public static void Unsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        EssentialCore.UIEvents.DoUnsubscribe(ref ticket);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void DoSend<T>(T eventData)
        where T : IUIEvent
    {
        this.aggregate.Send(eventData);
    }
    
    private BaseEventSubscriptionTicket DoSubscribe<TSpecific>(BaseEventAggregate<IUIEvent>.GameEventAction<TSpecific> actionDelegate, Func<TSpecific, bool> filterDelegate)
        where TSpecific : IUIEvent
    {
        return this.aggregate.Subscribe(actionDelegate, filterDelegate);
    }

    private void DoUnsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        this.aggregate.Unsubscribe(ref ticket);
    }
}