namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using Enums;

public class SBTNodeArrayInt : SBTNodeArray<int>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayInt(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, int entry)
    {
        writer.Write(entry);
    }

    protected override int DeserializeOne(BinaryReader reader)
    {
        return reader.ReadInt32();
    }
}
