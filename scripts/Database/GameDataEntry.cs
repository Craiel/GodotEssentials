namespace Craiel.Essentials.DB;

using Data;
using Resource;

public abstract class GameDataEntry<T> : IGameDataEntry
    where T: IGameDataId
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    protected GameDataEntry(T id)
    {
        this.Id = id;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly T Id;

    public GameDataState State { get; set; } = GameDataState.Development;
    
    public DataText DisplayName { get; set; }
    public DataText Description { get; set; }

    public ResourceKey IconSmall { get; set; }
    public ResourceKey IconLarge { get; set; }
}