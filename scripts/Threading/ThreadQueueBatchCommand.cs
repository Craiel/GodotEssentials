using System.Collections.Generic;
using Craiel.Essentials.Contracts;

namespace Craiel.Essentials.Threading;

public class ThreadQueueBatchCommand : IThreadQueueCommand
{
    private readonly Queue<IThreadQueueCommand> subCommands = new();
    private ThreadQueueModule host;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ThreadQueueBatchCommand(ThreadQueueModule host)
    {
        this.host = host;
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
        while (this.subCommands.Count > 0)
        {
            this.host.Queue(this.subCommands.Dequeue());
        }

        this.Suceeded = true;
        return true;
    }

    public void Enqueue(IThreadQueueCommand subCommand)
    {
        this.subCommands.Enqueue(subCommand);
    }
}