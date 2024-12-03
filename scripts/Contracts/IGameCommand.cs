namespace Craiel.Essentials.Contracts;

using Commands;

public interface IGameCommand
{
    GameCommandStatus Status { get; set; }

    void Execute();
}