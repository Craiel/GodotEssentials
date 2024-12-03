namespace Craiel.Essentials.Commands;

using Contracts;

public struct GameCommandEmptyPayload : IGameCommandPayload
{
    public static GameCommandEmptyPayload Value => new();
}