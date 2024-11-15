namespace Craiel.Essentials.Commands;

using Contracts;

public static class GameCommand
{
    public static void RegisterHandler<T>(GameCommands.CommandHandlerDelegate handler)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.Register<T>(handler);
    }
    
    public static void Queue<T>(IGameCommandPayload payload = null)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.Queue<T>(payload);
    }
    
    public static void ExecuteImmediate<T>(IGameCommandPayload payload = null)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.ExecuteImmediate<T>(payload);
    }
    
    public static void ExecuteImmediate<T>(T command, IGameCommandPayload payload = null)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.ExecuteImmediate(command, payload);
    }
}