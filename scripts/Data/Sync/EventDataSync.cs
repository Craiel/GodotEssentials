namespace Craiel.Essentials.Data;

using Craiel.Essentials.Contracts;

public class EventDataSync<T> : IGameEvent
    where T: ISyncedData
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public EventDataSync(T data)
    {
        this.Data = data;
    }

    public readonly T Data;
}