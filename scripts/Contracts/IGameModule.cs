namespace Craiel.Essentials.Contracts;

public interface IGameModule
{
    void Initialize();
    
    void Update(double delta);

    void Destroy();
}
