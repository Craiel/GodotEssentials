namespace Craiel.Essentials;

using Event;
using Events;
using Godot;

public partial class FloatingTextHandler : Node
{
    private BaseEventSubscriptionTicket uiEventShowFloatingText;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public Control Root;
    [Export] public PackedScene TextPrefab;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        
        // Subscribe to required events
        UIEvents.Subscribe<UIEventShowFloatingText>(OnShowText, out this.uiEventShowFloatingText);
    }

    public override void _ExitTree()
    {
        UIEvents.Unsubscribe(ref this.uiEventShowFloatingText);
        
        base._ExitTree();
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void OnShowText(UIEventShowFloatingText eventData)
    {
        var instance = this.TextPrefab.Instantiate<FloatingText>();
        instance.SetText(eventData.Text, eventData.Color, eventData.Flags);
        instance.Position = eventData.Position;
        
        this.Root.AddChild(instance);
    }
}