namespace Craiel.Essentials.Data.SBT;

using System.IO;
using Nodes;

public class SBTSet : SBTNodeList
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static SBTSet Deserialize(string data)
    {
        return (SBTSet)SBTUtils.Deserialize(data);
    }
    
    public static SBTSet Deserialize(byte[] data)
    {
        return (SBTSet)SBTUtils.Deserialize(data);
    }
    
    public static SBTSet DeserializeCompressed(byte[] data)
    {
        return (SBTSet)SBTUtils.DeserializeCompressed(data);
    }
    
    public static SBTSet Deserialize(Stream source)
    {
        return (SBTSet)SBTUtils.Deserialize(source);
    }
}