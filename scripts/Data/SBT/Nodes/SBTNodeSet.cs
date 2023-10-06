using System;
using System.Collections.Generic;

namespace Craiel.Essentials.Data.SBT.Nodes;

using System.IO;
using System.Text;
using Enums;

public class SBTNodeSet : ISBTNodeList
{
    private readonly IList<ISBTNode> entries = new List<ISBTNode>();

    private ushort currentPosition;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeSet(SBTFlags flags = SBTFlags.None, string note = null)
    {
        this.Flags = flags;
        this.Note = note;
        this.Encoding = Encoding.UTF8;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public Encoding Encoding { get; set; }
    
    public SBTFlags Flags { get; }
    
    public string Note { get; }

    public ushort Count
    {
        get { return (ushort)this.entries.Count; }
    }

    public SBTType Type
    {
        get { return SBTType.Set; }
    }
    
    public ISBTNode AddEntry(SBTType type, object data, SBTFlags flags, string note)
    {
        var node = SBTUtils.GetNode(type, data, flags, note);
        this.AddEntry(node);
        return node;
    }
    
    public void AddEntry<T>(T child)
        where T : ISBTNode
    {
        if (this.entries.Count >= ushort.MaxValue)
        {
            throw new InvalidDataException("Node Stream limit exceeded!");
        }
        
        this.entries.Add(child);
        this.currentPosition = (ushort)this.entries.Count;
    }
    
    public void Seek(ushort index)
    {
        if (index >= this.entries.Count)
        {
            throw new InvalidOperationException($"Attempt to seek beyond stream: {index} -> {this.entries.Count}");
        }
        
        currentPosition = index;
    }
    
    public T ReadNext<T>()
        where T : ISBTNode
    {
        if (this.entries.Count == 0)
        {
            throw new InvalidOperationException("No Entries to read");
        }
        
        if (this.currentPosition >= this.entries.Count)
        {
            throw new InvalidOperationException("End of Stream");
        }
        
        return (T) this.entries[this.currentPosition++];
    }
    
    public ISBTNode ReadNext()
    {
        if (this.entries.Count == 0)
        {
            throw new InvalidOperationException("No Entries to read");
        }
        
        if (this.currentPosition >= this.entries.Count)
        {
            throw new InvalidOperationException("End of Stream");
        }

        return this.entries[this.currentPosition++];
    }
    
    public bool TryReadNext<T>(out T result)
        where T : ISBTNode
    {
        result = default;
        
        if (this.entries.Count == 0)
        {
            throw new InvalidOperationException("No Entries to read");
        }

        if (this.currentPosition >= this.entries.Count)
        {
            throw new InvalidOperationException("End of Stream");
        }
        
        ISBTNode node = this.entries[this.currentPosition++];
        if (node is T)
        {
            result = (T) node;
            return true;
        }

        return false;
    }
    
    public void Save(BinaryWriter writer)
    {
        writer.Write((ushort)this.entries.Count);
        for (var i = 0; i < this.entries.Count; i++)
        {
            ISBTNode child = this.entries[i];
            child.WriteHeader(writer);
            child.Save(writer);
        }
    }
    
    public void Load(BinaryReader reader)
    {
        ushort count = reader.ReadUInt16();
        for (var i = 0; i < count; i++)
        {
            SBTType type;
            SBTFlags flags;
            SBTUtils.ReadHeader(reader, out type, out flags);

            object data = null;
            if (type.IsSimpleType())
            {
                data = SBTUtils.ReadSimpleTypeData(type, reader);
            }
            
            ISBTNode child = SBTUtils.GetNode(type, data, flags);
            if (!type.IsSimpleType())
            {
                child.Load(reader);
            }
            
            this.AddEntry(child);
        }

        currentPosition = 0;
    }
}