namespace Craiel.Essentials.Threading;

using System;
using System.Collections.Generic;
using Contracts;

public class ThreadQueueModule<T> : IEngineThreadModule
    where T : IThreadQueueCommand
{
    private static readonly long OperationWarningTimespan = TimeSpan.FromSeconds(2).Ticks;
    private static readonly long OperationErrorTimespan = TimeSpan.FromSeconds(5).Ticks;

    private readonly Queue<T> queuedCommands = new();
    private readonly List<T> lastFrameCommands = new();

    private long lastUpdateFrameTime;

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
        
        this.lastFrameCommands.Clear();
        while (this.queuedCommands.Count > 0)
        {
            T command = this.queuedCommands.Dequeue();
            command.Execute(lastUpdateFrameTime);
            this.lastFrameCommands.Add(command);
        }

#if DEBUG
        this.CheckLastUpdateOperations();
#endif
    }

    public void ExecuteImmediate(T command)
    {
        command.Execute(this.lastUpdateFrameTime);
        this.lastFrameCommands.Add(command);
    }
    
    public void Queue(T command)
    {
        if (command == null)
        {
            throw new ArgumentException();
        }

        command.QueueTime = this.lastUpdateFrameTime;

        this.queuedCommands.Enqueue(command);
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    private void CheckLastUpdateOperations()
    {
        int slowWarning = 0;
        int slowError = 0;
        int error = 0;

        foreach (var operation in this.lastFrameCommands)
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
