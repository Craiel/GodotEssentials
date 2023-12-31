namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using Enums;

public class SBTNodeArrayUShort : SBTNodeArray<ushort>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayUShort(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, ushort entry)
    {
        writer.Write(entry);
    }

    protected override ushort DeserializeOne(BinaryReader reader)
    {
        return reader.ReadUInt16();
    }
}
