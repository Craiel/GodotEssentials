namespace Craiel.Essentials.DB;

using Data;
using Resource;

public abstract class GameDataEntry<T>
    where T: struct
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
    
    public DataText DisplayName;
    public DataText Description;
    
    public ResourceKey IconSmall;
    public ResourceKey IconLarge;
}