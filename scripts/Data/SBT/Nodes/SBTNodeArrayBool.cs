using System.IO;
using Craiel.Essentials.Enums;

namespace Craiel.Essentials.Data.SBT.Nodes;

public class SBTNodeArrayBool : SBTNodeArray<bool>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayBool(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, bool entry)
    {
        writer.Write(entry);
    }

    protected override bool DeserializeOne(BinaryReader reader)
    {
        return reader.ReadBoolean();
    }
}