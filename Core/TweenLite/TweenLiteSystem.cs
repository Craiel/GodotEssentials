namespace Craiel.Essentials.Runtime.TweenLite;

using EngineCore;
using Singletons;

public partial class TweenLiteSystem : GodotSingleton<TweenLiteSystem>
{
    private readonly TicketProviderManaged<TweenLiteTicket, TweenLiteNode> activeTweens;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TweenLiteSystem()
    {
        this.activeTweens = new TicketProviderManaged<TweenLiteTicket, TweenLiteNode>();
        this.activeTweens.EnableManagedTickets(this.CheckTweenFinished, this.TweenFinished);
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void StartTween(TweenLiteNode node)
    {
        this.activeTweens.Register(node.Ticket, node);
        this.activeTweens.Manage(node.Ticket);
    }

    public void StopTween(ref TweenLiteTicket ticket)
    {
        this.activeTweens.Unregister(ticket);
        ticket = TweenLiteTicket.Invalid;
    }

    public void Update(double delta)
    {
        this.activeTweens.Update(delta);
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void TweenFinished(ref TweenLiteTicket ticket)
    {
        ticket = TweenLiteTicket.Invalid;
    }

    private bool CheckTweenFinished(TweenLiteTicket ticket)
    {
        if (this.activeTweens.TryGet(ticket, out TweenLiteNode data))
        {
            return data.IsFinished;
        }

        return true;
    }
}