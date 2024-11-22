namespace Craiel.Essentials.Input;

using Craiel.Essentials.Contracts;

public class EventSetInputLock : IGameEvent
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public EventSetInputLock(InputLockState state)
    {
        this.State = state;
    }

    public InputLockState State;
}