namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using Enums;

public class SBTNodeArrayDouble : SBTNodeArray<double>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeArrayDouble(SBTType type, SBTFlags flags, string note = null) 
        : base(type, flags, note)
    {
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void SerializeOne(BinaryWriter writer, double entry)
    {
        writer.Write(entry);
    }

    protected override double DeserializeOne(BinaryReader reader)
    {
        return reader.ReadDouble();
    }
}