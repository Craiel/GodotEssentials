namespace Craiel.Essentials.Runtime.Events;

using Contracts;

public class EventSceneTransitionStarting : IGameEvent
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public EventSceneTransitionStarting(object type)
    {
        this.Type = type;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public object Type { get; private set; }
}