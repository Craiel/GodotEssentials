namespace Craiel.Essentials.Contracts;

using System;

public interface IThreadQueueCommand
{
    bool Suceeded { get; set; }

    long QueueTime { get; set; }
    
    long ExecutionTime { get; set; }
    
    Func<IThreadQueueCommandPayload, bool> Action { get; }

    IThreadQueueCommandPayload Payload { get; set; }
}