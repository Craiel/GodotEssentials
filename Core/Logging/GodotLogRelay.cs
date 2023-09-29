namespace Craiel.Essentials.Runtime.Logging;

using Godot;

public class GodotLogRelay
{
    public void Info(string message)
    {
        GD.Print(message);
    }
    
    public void Warn(string message)
    {
        GD.PushWarning(message);
    }

    public void Error(string message)
    {
        GD.PushError(message);
    }
    
    public void Error<T>(string message, T exception = null)
        where T: Exception
    {
        GD.PushError(message);
    }
}