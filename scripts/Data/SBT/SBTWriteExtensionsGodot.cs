namespace Craiel.Essentials.Data.SBT;

using Enums;
using Godot;
using Nodes;

public static class SBTWriteExtensionsGodot
{
    public static SBTNodeList Add(this SBTNodeList target, Vector2 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Vector2, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Vector2 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Vector2, data, flags, note);
        return target;
    }
    
    public static SBTNodeList Add(this SBTNodeList target, Vector3 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Vector3, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Vector3 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Vector3, data, flags, note);
        return target;
    }
    
    public static SBTNodeList Add(this SBTNodeList target, Quaternion data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Quaternion, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Quaternion data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Quaternion, data, flags, note);
        return target;
    }
    
    public static SBTNodeList Add(this SBTNodeList target, Color data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Color, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Color data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Color, data, flags, note);
        return target;
    }
}