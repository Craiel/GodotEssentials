namespace Craiel.Essentials.Runtime.EngineCore;

public interface IGameModule
{
    void Initialize();
    
    void Update(double delta);

    void Destroy();
}
