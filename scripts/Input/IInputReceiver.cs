namespace Craiel.Essentials.Input;

public interface IInputReceiver
{
    public bool InputIgnoreLock { get; }
    
    public void ProcessInput();
}