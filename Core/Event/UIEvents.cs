namespace Craiel.Essentials.Runtime.Event;

using System;
using Singletons;

public class UIEvents : GodotSingleton<UIEvents>
{
    private BaseEventAggregate<IUIEvent> aggregate;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Initialize()
    {
        base.Initialize();

        this.aggregate = new BaseEventAggregate<IUIEvent>();
    }

    public static void Send<T>(T eventData)
        where T : IUIEvent
    {
        if (IsInstanceActive)
        {
            Instance.DoSend(eventData);
        }
    }

    public static void Subscribe<TSpecific>(BaseEventAggregate<IUIEvent>.GameEventAction<TSpecific> actionDelegate, 
        out BaseEventSubscriptionTicket ticket, 
        Func<TSpecific, bool> filterDelegate = null)
        where TSpecific : IUIEvent
    {
        ticket = null;
        if (IsInstanceActive)
        {
            ticket = Instance.DoSubscribe(actionDelegate, filterDelegate);
        }
    }
    
    public static void Unsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        if (IsInstanceActive)
        {
            Instance.DoUnsubscribe(ref ticket);
        }
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