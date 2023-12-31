namespace Craiel.Essentials.Data.SBT.Nodes;

using System.Collections.Generic;
using System.IO;
using Enums;
using SBT;

public class SBTNodeList : ISBTNodeList
{
    private readonly IList<ISBTNode> children;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public SBTNodeList(SBTFlags flags = SBTFlags.None, string note = null)
    {
        this.children = new List<ISBTNode>();
        
        this.Flags = flags;
        this.Note = note;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public ushort Count
    {
        get { return (ushort)this.children.Count; }
    }
    
    public SBTFlags Flags { get; set; }
    
    public string Note { get; }

    public SBTType Type
    {
        get { return SBTType.List; }
    }
    
    public ISBTNode AddEntry(SBTType type, object data = null, SBTFlags flags = SBTFlags.None, string note = null)
    {
        var node = SBTUtils.GetNode(type, data, flags, note);
        this.AddEntry(node);
        return node;
    }
    
    public void AddEntry<T>(T child)
        where T : ISBTNode
    {
        if (this.children.Count >= ushort.MaxValue)
        {
            throw new InvalidDataException("Node List limit exceeded!");
        }
        
        this.children.Add(child);
    }

    public T Read<T>(int index)
        where T : ISBTNode
    {
        return (T) this.children[index];
    }
    
    public ISBTNode Read(int index)
    {
        return this.children[index];
    }
    
    public bool TryRead<T>(int index, out T result)
        where T : ISBTNode
    {
        result = default;
        if (index >= this.children.Count)
        {
            return false;
        }

        ISBTNode node = this.children[index];
        if (node is T)
        {
            result = (T) node;
            return true;
        }

        return false;
    }
    
    public void Save(BinaryWriter writer)
    {
        writer.Write((ushort)this.children.Count);
        for (var i = 0; i < this.children.Count; i++)
        {
            ISBTNode child = this.children[i];
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
    }
}