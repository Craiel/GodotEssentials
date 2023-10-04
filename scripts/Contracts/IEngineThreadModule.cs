using Craiel.Essentials.Threading;

namespace Craiel.Essentials.Contracts;

public interface IEngineThreadModule
{
    public void Update(EngineTime time);
}