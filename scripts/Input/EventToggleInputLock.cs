namespace Craiel.Essentials.Input;

using Craiel.Essentials.Contracts;

public class EventToggleInputLock : IGameEvent
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public EventToggleInputLock(bool isLocked)
    {
        this.IsLocked = isLocked;
    }

    public bool IsLocked;
}