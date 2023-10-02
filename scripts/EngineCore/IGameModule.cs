namespace Craiel.Essentials.EngineCore;

public interface IGameModule
{
    void Initialize();
    
    void Update(double delta);

    void Destroy();
}
