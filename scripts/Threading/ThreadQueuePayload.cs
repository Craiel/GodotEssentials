namespace Craiel.Essentials.Threading;

using Contracts;

public struct EmptyThreadQueuePayload : IThreadQueueCommandPayload
{
    public static EmptyThreadQueuePayload Value = new();
}
