namespace Craiel.Essentials.DB;

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
    
    public string DisplayName;
    public string Description;
    
    public ResourceKey IconSmall;
    public ResourceKey IconLarge;
}