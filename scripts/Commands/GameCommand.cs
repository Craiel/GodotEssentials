namespace Craiel.Essentials.Commands;

using Contracts;

public static class GameCommand
{
    public static void Queue<T>()
        where T: IGameCommand
    {
        EssentialCore.GameCommands.Queue<T>();
    }
    
    public static void Queue<T>(T command)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.Queue(command);
    }
    
    public static void ExecuteImmediate<T>(T command)
        where T: IGameCommand
    {
        EssentialCore.GameCommands.ExecuteImmediate<T>(command);
    }
    
    public static void ExecuteImmediate<T>()
        where T: IGameCommand
    {
        EssentialCore.GameCommands.ExecuteImmediate<T>();
    }
}