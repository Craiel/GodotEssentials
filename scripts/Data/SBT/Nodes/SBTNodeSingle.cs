namespace Craiel.Essentials.Data.SBT.Nodes;

using System;
using System.Globalization;
using System.IO;
using Enums;

public struct SBTNodeSingle : ISBTNode
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeSingle(float data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        this.Data = data;
        this.Flags = flags;
        this.Note = note;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly float Data;
    
    public SBTFlags Flags { get; }
    
    public string Note { get; }

    public SBTType Type
    {
        get { return SBTType.Single; }
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