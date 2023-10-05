namespace Craiel.Essentials.Threading;

using System;
using Contracts;

public class CallbackThreadQueueCommand<T> : IThreadQueueCommand
    where T: IThreadQueueCommandPayload
{
    private Func<T, bool> callback;
    private T payload;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public CallbackThreadQueueCommand(Func<T, bool> callback, T payload)
    {
        this.callback = callback;
        this.payload = payload;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public bool Suceeded { get; private set; }

    public long QueueTime { get; set; }

    public long ExecutionTime { get; private set; }

    public bool Execute(long time)
    {
        this.ExecutionTime = time;
        this.Suceeded = this.callback.Invoke(this.payload);
        return this.Suceeded;
    }
}