namespace Craiel.Essentials.Threading;

using System;
using System.Collections.Generic;
using Contracts;

public abstract class ThreadQueuedComponent : IThreadQueueComponent
{
    private static readonly long OperationWarningTimespan = TimeSpan.FromSeconds(2).Ticks;
    private static readonly long OperationErrorTimespan = TimeSpan.FromSeconds(5).Ticks;

    private Queue<IThreadQueueOperation> queuedOperations;
    private List<IThreadQueueOperation> lastOperations;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public bool HasQueuedOperations
    {
        get
        {
            if (this.queuedOperations == null || this.queuedOperations.Count > 0)
            {
                return false;
            }

            return true;
        }
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected void ProcessOperations(long time)
    {
        if (this.queuedOperations != null)
        {
            this.lastOperations = new List<IThreadQueueOperation>();
            while (this.queuedOperations.Count > 0)
            {
                IThreadQueueOperation operation = this.queuedOperations.Dequeue();
                operation.Suceeded = operation.Action(operation.Payload);
                operation.ExecutionTime = time;
                this.lastOperations.Add(operation);
            }

#if DEBUG
            this.CheckLastUpdateOperations();
#endif
        }
    }

    protected void QueueOperation(Func<IThreadQueueOperationPayload, bool> action, long time)
    {
        if (action == null)
        {
            throw new ArgumentException();
        }

        if (this.queuedOperations == null)
        {
            this.queuedOperations = new Queue<IThreadQueueOperation>();
        }

        var operation = new ThreadQueueOperation(action, time);
        this.queuedOperations.Enqueue(operation);
    }

    private void CheckLastUpdateOperations()
    {
        int slowWarning = 0;
        int slowError = 0;
        int error = 0;

        foreach (IThreadQueueOperation operation in this.lastOperations)
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
