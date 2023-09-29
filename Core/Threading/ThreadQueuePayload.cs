namespace Craiel.Essentials.Runtime.Threading;

using Contracts;

public class ThreadQueuePayload : IThreadQueueOperationPayload
{
    public object Data { get; set; }
}
