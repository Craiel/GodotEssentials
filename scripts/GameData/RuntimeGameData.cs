namespace Craiel.Essentials.GameData;

using System;
using Godot;
using Resource;

[Serializable]
public abstract class RuntimeGameData
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public GameDataId Id;

    public string Name;

    public string DisplayName;

    public string Description;
    
    public string IconSmallResourcePath;

    public string IconLargeResourcePath;
    
    public ResourceKey IconSmall { get; private set; }
    
    public ResourceKey IconLarge { get; private set; }
    
    public virtual void PostLoad()
    {
        if (!string.IsNullOrEmpty(this.IconSmallResourcePath))
        {
            this.IconSmall = ResourceKey.Create<Texture>(this.IconSmallResourcePath);
        }
        
        if (!string.IsNullOrEmpty(this.IconLargeResourcePath))
        {
            this.IconLarge = ResourceKey.Create<Texture>(this.IconLargeResourcePath);
        }
    }
}