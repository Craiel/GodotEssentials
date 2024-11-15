namespace Craiel.Essentials.Data.SBT;

using System;
using Enums;
using Godot;
using Nodes;
using Utils;

public static class ISBTNodeListExtensions
{
    public static ISBTNodeList Add(this ISBTNodeList target, string data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.String, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, bool data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Bool, data, flags, note);
        return target;
    }
    
    public static ISBTNodeList Add(this ISBTNodeList target, byte data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Byte, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, short data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Short, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, ushort data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.UShort, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, int data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Int, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, uint data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.UInt, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, long data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Long, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, ulong data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.ULong, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, float data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Single, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, double data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Double, data, flags, note);
        return target;
    }

    public static SBTNodeList StartList(this ISBTNodeList target, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeList)target.AddEntry(SBTType.List, null, flags, note);
    }

    public static SBTNodeDictionary StartDictionary(this ISBTNodeList target, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeDictionary)target.AddEntry(SBTType.Dictionary, null, flags, note);
    }

    public static SBTNodeArray<T> StartArray<T>(this ISBTNodeList target, SBTFlags flags = SBTFlags.None, string note = null)
    {
        SBTType type = SBTUtils.GetArrayBaseType(TypeDef<T>.Value);
        return (SBTNodeArray<T>)target.AddEntry(type, null, flags, note);
    }

    public static void AddArray<T>(this ISBTNodeList target, T[] values, SBTFlags flags = SBTFlags.None, string note = null)
    {
        SBTType type = SBTUtils.GetArrayBaseType(TypeDef<T>.Value);
        var array = (SBTNodeArray<T>) target.AddEntry(type, null, flags, note);
        array.Add(values);
    }

    public static SBTNodeSet StartSet(this ISBTNodeList target, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeSet)target.AddEntry(SBTType.Set, null, flags, note);
    }

    public static SBTNodeStream StartStream(this ISBTNodeList target, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeStream)target.AddEntry(SBTType.Stream, null, flags, note);
    }

    public static ISBTNodeList Add(this ISBTNodeList target, DateTime data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.DateTime, data, flags, note);
        return target;
    }

    public static ISBTNodeList Add(this ISBTNodeList target, TimeSpan data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.TimeSpan, data, flags, note);
        return target;
    }
    
    public static ISBTNodeList Add(this ISBTNodeList target, Vector2 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Vector2, data, flags, note);
        return target;
    }
    
    public static ISBTNodeList Add(this ISBTNodeList target, Vector3 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Vector3, data, flags, note);
        return target;
    }
    
    public static ISBTNodeList Add(this ISBTNodeList target, Quaternion data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Quaternion, data, flags, note);
        return target;
    }
    
    public static ISBTNodeList Add(this ISBTNodeList target, Color data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(SBTType.Color, data, flags, note);
        return target;
    }
}