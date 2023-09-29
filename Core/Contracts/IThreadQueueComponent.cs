namespace Craiel.Essentials.Runtime.Contracts;

public interface IThreadQueueComponent
{
    bool HasQueuedOperations { get; }
}
