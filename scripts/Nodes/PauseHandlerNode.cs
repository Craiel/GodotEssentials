namespace Craiel.Essentials.Nodes;

using Event;
using Events;
using Godot;

public partial class PauseHandlerNode : Node
{
    private BaseEventSubscriptionTicket pauseRequestEventTicket;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public Node Target;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        GameEvents.Subscribe<EventPauseRequest>(this.OnPauseRequest, out this.pauseRequestEventTicket);
    }

    public override void _ExitTree()
    {
        GameEvents.Unsubscribe(ref this.pauseRequestEventTicket);
        
        base._ExitTree();
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void OnPauseRequest(EventPauseRequest eventData)
    {
        this.Target.GetTree().Paused = eventData.RequestedState;
    }
}