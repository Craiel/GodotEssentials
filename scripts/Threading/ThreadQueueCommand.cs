namespace Craiel.Essentials.Threading;

using System;
using Contracts;

public class ThreadQueueCommand : IThreadQueueCommand
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ThreadQueueCommand(Func<IThreadQueueCommandPayload, bool> action, long queueTime)
    {
        this.Action = action;
        this.QueueTime = queueTime;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public bool Suceeded { get; set; }

    public Func<IThreadQueueCommandPayload, bool> Action { get; private set; }

    public IThreadQueueCommandPayload Payload { get; set; }

    public long QueueTime { get; set; }

    public long ExecutionTime { get; set; }
}