namespace Craiel.Essentials.EngineCore;

using System.Collections.Generic;
using Contracts;
using Event;

public class GameModuleBase<T> : IGameModule
    where T: IGameModule
{
    private readonly IList<BaseEventSubscriptionTicket> managedEventSubscriptions;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    protected GameModuleBase(T parent)
    {
        this.Parent = parent;
        this.managedEventSubscriptions = new List<BaseEventSubscriptionTicket>();
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public virtual void Initialize()
    {
    }

    public virtual void Update(double delta)
    {
    }

    public virtual void Destroy()
    {
        foreach (BaseEventSubscriptionTicket ticket in this.managedEventSubscriptions)
        {
            BaseEventSubscriptionTicket closure = ticket;
            GameEvents.Unsubscribe(ref closure);
        }
        
        this.managedEventSubscriptions.Clear();
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected T Parent;

    protected void SubscribeEvent<TE>(BaseEventAggregate<IGameEvent>.GameEventAction<TE> callback)
        where TE : IGameEvent
    {
        GameEvents.Subscribe(callback, out BaseEventSubscriptionTicket ticket);
        
        this.managedEventSubscriptions.Add(ticket);
    }
}
