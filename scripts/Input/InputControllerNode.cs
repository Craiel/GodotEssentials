﻿namespace Craiel.Essentials.Input;

using Craiel.Essentials;
using Craiel.Essentials.Event;
using Godot;

public partial class InputControllerNode : Node
{
    private BaseEventSubscriptionTicket eventInputLockToggle;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _EnterTree()
    {
        GameEvents.Subscribe<EventToggleInputLock>(OnToggleInputLock, out this.eventInputLockToggle);
		
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        EssentialCore.Destroy();
		
        GameEvents.Unsubscribe(ref this.eventInputLockToggle);
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void OnToggleInputLock(EventToggleInputLock eventData)
    {
        InputController.InputLocked = eventData.IsLocked;
    }
}