namespace Craiel.Essentials.Contracts;

public interface IThreadQueueComponent
{
    bool HasQueuedOperations { get; }
}
