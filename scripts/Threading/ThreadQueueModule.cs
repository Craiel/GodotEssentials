namespace Craiel.Essentials.Threading;

using System;
using System.Collections.Generic;
using Contracts;

public class ThreadQueueModule : IEngineThreadModule
{
    private static readonly long OperationWarningTimespan = TimeSpan.FromSeconds(2).Ticks;
    private static readonly long OperationErrorTimespan = TimeSpan.FromSeconds(5).Ticks;

    private readonly Queue<IThreadQueueCommand> queuedCommands;
    private readonly List<IThreadQueueCommand> lastCommand;
    private readonly Stack<ThreadQueueBatchCommand> batchStack;

    private long lastUpdateFrameTime;

    public ThreadQueueModule()
    {
        this.queuedCommands = new Queue<IThreadQueueCommand>();
        this.lastCommand = new List<IThreadQueueCommand>();
        this.batchStack = new Stack<ThreadQueueBatchCommand>();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public bool HasQueuedOperations
    {
        get
        {
            if (this.queuedCommands.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
    
    public void Update(EngineTime time)
    {
        lastUpdateFrameTime = time.Ticks;
        
        if (this.queuedCommands != null)
        {
            this.lastCommand.Clear();
            while (this.queuedCommands.Count > 0)
            {
                IThreadQueueCommand command = this.queuedCommands.Dequeue();
                command.Execute(time.Ticks);
                this.lastCommand.Add(command);
            }

#if DEBUG
            this.CheckLastUpdateOperations();
#endif
        }
    }

    public void Queue(IThreadQueueCommand command)
    {
        if (command == null)
        {
            throw new ArgumentException();
        }

        command.QueueTime = this.lastUpdateFrameTime;

        if (this.batchStack.TryPeek(out ThreadQueueBatchCommand batch))
        {
            batch.Enqueue(command);
        }
        else
        {
            this.queuedCommands.Enqueue(command);
        }
    }
    
    public void Queue<T>(Func<T, bool> action, T payload)
        where T: IThreadQueueCommandPayload
    {
        if (action == null)
        {
            throw new ArgumentException();
        }

        var command = new CallbackThreadQueueCommand<T>(action, payload)
        {
            QueueTime = this.lastUpdateFrameTime
        };

        if (this.batchStack.TryPeek(out ThreadQueueBatchCommand batch))
        {
            batch.Enqueue(command);
        }
        else
        {
            this.queuedCommands.Enqueue(command);
        }
    }

    public void BeginBatch()
    {
        // Create a new batch command, no queue at this time, EndBatch() will queue this up 
        var newBatch = new ThreadQueueBatchCommand(this);
        this.batchStack.Push(newBatch);
    }

    public void EndBatch()
    {
        if (!this.batchStack.TryPop(out ThreadQueueBatchCommand batch))
        {
            throw new InvalidOperationException("No active Command Batch!");
        }
        
        // Queue up the batch now, this can cascade though multiple batches if needed
        this.Queue(batch);
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    private void CheckLastUpdateOperations()
    {
        int slowWarning = 0;
        int slowError = 0;
        int error = 0;

        foreach (IThreadQueueCommand operation in this.lastCommand)
        {
            long timeToUpdate = operation.ExecutionTime - operation.QueueTime;
            if (timeToUpdate > OperationErrorTimespan)
            {
                slowError++;
            }

            if (timeToUpdate > OperationWarningTimespan)
            {
                slowWarning++;
            }

            if (!operation.Suceeded)
            {
#if DEBUG
                EssentialCore.Logger.Error($"Operation did not Succeed: {operation.GetType().Name}");
#endif
                
                error++;
            }
        }

        if (error > 0)
        {
            EssentialCore.Logger.Error($"[{this.GetType()}] {error} operations in {error} had errors!");
        }

        if (slowError > 0)
        {
            EssentialCore.Logger.Error($"[{this.GetType()}] {slowError} operations in {slowError} took longer then expected!");
        }

        if (slowWarning > 0)
        {
            EssentialCore.Logger.Error($"[{this.GetType()}] Operation in {slowWarning} took more than 2 seconds to complete");
        }
    }
}
