namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using Enums;

public class SBTNodeArrayULong : SBTNodeArray<ulong>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayULong(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, ulong entry)
    {
        writer.Write(entry);
    }

    protected override ulong DeserializeOne(BinaryReader reader)
    {
        return reader.ReadUInt64();
    }
}
