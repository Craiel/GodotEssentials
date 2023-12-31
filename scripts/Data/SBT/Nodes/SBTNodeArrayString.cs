namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using Enums;

public class SBTNodeArrayString : SBTNodeArray<string>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayString(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, string entry)
    {
        writer.Write(entry);
    }

    protected override string DeserializeOne(BinaryReader reader)
    {
        return reader.ReadString();
    }
}
