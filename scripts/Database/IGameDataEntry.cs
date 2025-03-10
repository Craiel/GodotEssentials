namespace Craiel.Essentials.DB;

using Data;
using Resource;

public interface IGameDataEntry
{
    public DataText DisplayName { get; }
    public DataText Description { get; }
    
    public ResourceKey IconSmall { get; }
    public ResourceKey IconLarge { get; }
}