namespace Craiel.Essentials.Commands;

using Contracts;

public abstract class GameCommandBase<T> : IGameCommand
    where T : IGameCommandPayload
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    protected GameCommandBase(T payload)
    {
        this.Payload = payload;
    }
    
    public GameCommandStatus Status { get; set; } = GameCommandStatus.NotRun;
    
    public abstract void Execute();

    protected readonly T Payload;
}