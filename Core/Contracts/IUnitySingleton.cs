namespace Craiel.Essentials.Runtime.Contracts;

public interface IGodotSingleton
{
    bool IsInitialized { get; }
    void Initialize();

    void DestroySingleton();
}