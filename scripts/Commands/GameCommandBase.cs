namespace Craiel.Essentials.Commands;

using Contracts;

public abstract class GameCommandBase : IGameCommand
{
    private GameCommandStatus status = GameCommandStatus.NotRun;
    
    public GameCommandStatus Status
    {
        get => this.status;
        set => this.status = value;
    }

    public abstract string Id { get; }
}