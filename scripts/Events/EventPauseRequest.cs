namespace Craiel.Essentials.Events;

using Craiel.Essentials.Contracts;

public class EventPauseRequest : IGameEvent
{
    public EventPauseRequest(bool pause)
    {
        this.RequestedState = pause;
    }

    public bool RequestedState;
}