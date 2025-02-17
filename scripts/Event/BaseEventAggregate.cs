namespace Craiel.Essentials.Event;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using DebugTools;
using Utils;

public class BaseEventAggregate<T> : IEventAggregate
    where T : class
{
    private readonly IDictionary<Type, BaseEventTargetCollection<T>> subscribers;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public BaseEventAggregate()
    {
        this.subscribers = new Dictionary<Type, BaseEventTargetCollection<T>>();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
#if DEBUG
    public readonly EventDebugTracker<T> DebugTracker = new();
#endif
    
    public delegate void GameEventAction<in TSpecific>(TSpecific eventData)
        where TSpecific : T;

    public BaseEventSubscriptionTicket Subscribe<TSpecific>(GameEventAction<TSpecific> actionDelegate, Func<TSpecific, bool> filterDelegate = null)
        where TSpecific : T
    {
        var ticket = new BaseEventSubscriptionTicket(TypeDef<TSpecific>.Value, actionDelegate);
        if (filterDelegate != null)
        {
            ticket.FilterDelegate = x => filterDelegate((TSpecific) x);
        }

        this.DoSubscribe(ticket);
        return ticket;
    }

    public void Unsubscribe(ref BaseEventSubscriptionTicket ticket)
    {
        if (ticket == null)
        {
            return;
        }

        this.DoUnsubscribe(ticket);
        ticket = null;
    }

    public void Send<TSpecific>(TSpecific eventData)
        where TSpecific : T
    {
        this.DoSend(eventData);
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected void DoSubscribe(BaseEventSubscriptionTicket ticket)
    {
        lock (this.subscribers)
        {
            if (!this.subscribers.TryGetValue(ticket.TargetType, out var targets))
            {
                targets = new BaseEventTargetCollection<T>();
                this.subscribers.Add(ticket.TargetType, targets);
            }

            targets.Add(ticket);
        }
    }

    protected void DoUnsubscribe(BaseEventSubscriptionTicket ticket)
    {
        lock (this.subscribers)
        {
            if (this.subscribers.TryGetValue(ticket.TargetType, out var targets)
                && targets.Remove(ticket))
            {
                return;
            }
            
            throw new InvalidOperationException("Unsubscribe could not find the given target");
        }
    }

    protected void DoSend<TSpecific>(TSpecific eventData)
        where TSpecific : T
    {
        lock (this.subscribers)
        {
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
            int receives = 0;
#endif
            
            if (this.subscribers.TryGetValue(TypeDef<TSpecific>.Value, out var targets))
            {
#if DEBUG
                receives++;
#endif
                
                targets.Send(eventData);
            }
            
#if DEBUG
            sw.Stop();
            this.DebugTracker.Track<TSpecific>(1, receives, sw.Elapsed.TotalSeconds);
#endif
        }
    }
}
