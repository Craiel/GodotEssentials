namespace Craiel.Essentials.Contracts;

public interface IThreadQueueCommand
{
    bool Suceeded { get; }

    long QueueTime { get; set; }
    
    long ExecutionTime { get;  }

    bool Execute(long time);
}