namespace Craiel.Essentials.Data.SBT;

using System.IO;
using Nodes;

public class SBTStream : SBTNodeList
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static SBTStream Deserialize(string data)
    {
        return (SBTStream)SBTUtils.Deserialize(data);
    }
    
    public static SBTStream Deserialize(byte[] data)
    {
        return (SBTStream)SBTUtils.Deserialize(data);
    }
    
    public static SBTStream DeserializeCompressed(byte[] data)
    {
        return (SBTStream)SBTUtils.DeserializeCompressed(data);
    }
    
    public static SBTStream Deserialize(Stream source)
    {
        return (SBTStream)SBTUtils.Deserialize(source);
    }
}