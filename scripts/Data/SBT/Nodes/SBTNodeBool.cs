using System;
using System.IO;
using Craiel.Essentials.Enums;

namespace Craiel.Essentials.Data.SBT.Nodes;

public struct SBTNodeBool : ISBTNode
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeBool(bool data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        this.Data = data;
        this.Flags = flags;
        this.Note = note;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly bool Data;
    
    public SBTFlags Flags { get; }
    
    public string Note { get; }

    public SBTType Type
    {
        get { return SBTType.Bool; }
    }
    
    public void Save(BinaryWriter writer)
    {
        writer.Write(this.Data);
    }
    
    public void Load(BinaryReader reader)
    {
        throw new InvalidOperationException();
    }
}