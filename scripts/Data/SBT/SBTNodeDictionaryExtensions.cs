using Craiel.Essentials.Utils;
using Godot;

namespace Craiel.Essentials.Data.SBT;

using System;
using Enums;
using Nodes;

public static class SBTNodeDictionaryExtensions
{
    public static string ReadString(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeString>(key).Data;
    }
    
    public static byte ReadByte(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeByte>(key).Data;
    }
    
    public static short ReadShort(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeShort>(key).Data;
    }
    
    public static ushort ReadUShort(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeUShort>(key).Data;
    }
    
    public static int ReadInt(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeInt>(key).Data;
    }
    
    public static uint ReadUInt(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeUInt>(key).Data;
    }
    
    public static long ReadLong(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeLong>(key).Data;
    }
    
    public static ulong ReadULong(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeULong>(key).Data;
    }
    
    public static float ReadSingle(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeSingle>(key).Data;
    }
    
    public static double ReadDouble(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeDouble>(key).Data;
    }
    
    public static SBTFlags ReadFlags(this SBTNodeDictionary source, string key)
    {
        return source.Read(key).Flags;
    }
    
    public static string ReadNote(this SBTNodeDictionary source, string key)
    {
        return source.Read(key).Note;
    }
    
    public static SBTNodeArray<T> ReadArray<T>(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeArray<T>>(key);
    }
    
    public static SBTNodeList ReadList(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeList>(key);
    }
    
    public static SBTNodeDictionary ReadDictionary(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeDictionary>(key);
    }
    
    public static SBTNodeSet ReadSet(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeSet>(key);
    }

    public static SBTNodeStream ReadStream(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeStream>(key);
    }
    
    public static DateTime ReadDateTime(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeDateTime>(key).Data;
    }
    
    public static TimeSpan ReadTimeSpan(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeTimeSpan>(key).Data;
    }
    
    public static bool TryReadString(this SBTNodeDictionary source, string key, out string result)
    {
        result = null;
        if (source.TryRead(key, out SBTNodeString node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static string TryReadString(this SBTNodeDictionary source, string key, string defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeString node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadByte(this SBTNodeDictionary source, string key, out byte result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeByte node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static byte TryReadByte(this SBTNodeDictionary source, string key, byte defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeByte node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadShort(this SBTNodeDictionary source, string key, out short result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static short TryReadShort(this SBTNodeDictionary source, string key, short defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeShort node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUShort(this SBTNodeDictionary source, string key, out ushort result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeUShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ushort TryReadUShort(this SBTNodeDictionary source, string key, ushort defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeUShort node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadInt(this SBTNodeDictionary source, string key, out int result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }

    public static int TryReadInt(this SBTNodeDictionary source, string key, int defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUInt(this SBTNodeDictionary source, string key, out uint result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeUInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static uint TryReadUInt(this SBTNodeDictionary source, string key, uint defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeUInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadLong(this SBTNodeDictionary source, string key, out long result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeLong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static long TryReadLong(this SBTNodeDictionary source, string key, long defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeLong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadULong(this SBTNodeDictionary source, string key, out ulong result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeULong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ulong TryReadULong(this SBTNodeDictionary source, string key, ulong defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeULong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadSingle(this SBTNodeDictionary source, string key, out float result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeSingle node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static float TryReadSingle(this SBTNodeDictionary source, string key, float defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeSingle node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadDouble(this SBTNodeDictionary source, string key, out double result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeDouble node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static double TryReadDouble(this SBTNodeDictionary source, string key, double defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeDouble node))
        {
            return node.Data;
        }

        return defaultValue;
    }

    public static bool TryReadFlags(this SBTNodeDictionary source, string key, out SBTFlags result)
    {
        result = default;
        if (source.TryRead(key, out ISBTNode node))
        {
            result = node.Flags;
            return true;
        }

        return false;
    }
    
    public static SBTFlags TryReadFlags(this SBTNodeDictionary source, string key, SBTFlags defaultValue = default)
    {
        if (source.TryRead(key, out ISBTNode node))
        {
            return node.Flags;
        }

        return defaultValue;
    }
    
    public static bool TryReadArray<T>(this SBTNodeDictionary source, string key, out SBTNodeArray<T> result)
    {
        return source.TryRead(key, out result);
    }
    
    public static SBTNodeArray<T> TryReadArray<T>(this SBTNodeDictionary source, string key, SBTNodeArray<T> defaultValue = default)
    {
        return source.TryRead(key, out SBTNodeArray<T> result) ? result : defaultValue;
    }
    
    public static bool TryReadList(this SBTNodeDictionary source, string key, out SBTNodeList result)
    {
        return source.TryRead(key, out result);
    }
    
    public static SBTNodeList TryReadList(this SBTNodeDictionary source, string key, SBTNodeList defaultValue)
    {
        return source.TryRead(key, out SBTNodeList result) ? result : defaultValue;
    }
    
    public static bool TryReadDictionary(this SBTNodeDictionary source, string key, out SBTNodeDictionary result)
    {
        return source.TryRead(key, out result);
    }
    
    public static SBTNodeDictionary TryReadDictionary(this SBTNodeDictionary source, string key, SBTNodeDictionary defaultValue)
    {
        return source.TryRead(key, out SBTNodeDictionary result) ? result : defaultValue;
    }
    
    public static bool TryReadSet(this SBTNodeDictionary source, string key, out SBTNodeSet result)
    {
        return source.TryRead(key, out result);
    }
    
    public static SBTNodeSet TryReadSet(this SBTNodeDictionary source, string key, SBTNodeSet defaultValue)
    {
        return source.TryRead(key, out SBTNodeSet result) ? result : defaultValue;
    }
    
    public static bool TryReadStream(this SBTNodeDictionary source, string key, out SBTNodeStream result)
    {
        return source.TryRead(key, out result);
    }
    
    public static SBTNodeStream TryReadStream(this SBTNodeDictionary source, string key, SBTNodeStream defaultValue)
    {
        return source.TryRead(key, out SBTNodeStream result) ? result : defaultValue;
    }
    
    public static bool TryReadDateTime(this SBTNodeDictionary source, string key, out DateTime result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeDateTime node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static DateTime TryReadDateTime(this SBTNodeDictionary source, string key, DateTime defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeDateTime node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadTimeSpan(this SBTNodeDictionary source, string key, out TimeSpan result)
    {
        result = default;
        if (source.TryRead(key, out SBTNodeTimeSpan node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static TimeSpan TryReadTimeSpan(this SBTNodeDictionary source, string key, TimeSpan defaultValue = default)
    {
        if (source.TryRead(key, out SBTNodeTimeSpan node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static Vector2 ReadVector2(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeVector2>(key).Data;
    }
    
    public static Vector3 ReadVector3(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeVector3>(key).Data;
    }
    
    public static Quaternion ReadQuaternion(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeQuaternion>(key).Data;
    }
    
    public static Color ReadColor(this SBTNodeDictionary source, string key)
    {
        return source.Read<SBTNodeColor>(key).Data;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, string data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.String, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, bool data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Bool, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, byte data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Byte, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, short data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Short, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, ushort data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.UShort, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, int data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Int, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, uint data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.UInt, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, long data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Long, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, ulong data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.ULong, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, float data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Single, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, double data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Double, data, flags, note);
        return target;
    }

    public static SBTNodeList StartList(this SBTNodeDictionary target, string key, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeList)target.AddEntry(key, SBTType.List, null, flags, note);
    }

    public static SBTNodeDictionary StartDictionary(this SBTNodeDictionary target, string key, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeDictionary)target.AddEntry(key, SBTType.Dictionary, null, flags, note);
    }

    public static SBTNodeArray<T> StartArray<T>(this SBTNodeDictionary target, string key, SBTFlags flags = SBTFlags.None, string note = null)
    {
        SBTType type = SBTUtils.GetArrayBaseType(TypeDef<T>.Value);
        return (SBTNodeArray<T>)target.AddEntry(key, type, null, flags, note);
    }

    public static void AddArray<T>(this SBTNodeDictionary target, string key, T[] values, SBTFlags flags = SBTFlags.None, string note = null)
    {
        SBTType type = SBTUtils.GetArrayBaseType(TypeDef<T>.Value);
        var array = (SBTNodeArray<T>) target.AddEntry(key, type, null, flags, note);
        array.SetCapacity(values.Length);
        array.Add(values);
    }

    public static SBTNodeSet StartSet(this SBTNodeDictionary target, string key, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeSet)target.AddEntry(key, SBTType.Set, null, flags, note);
    }
    
    public static SBTNodeStream StartStream(this SBTNodeDictionary target, string key, SBTFlags flags = SBTFlags.None, string note = null)
    {
        return (SBTNodeStream)target.AddEntry(key, SBTType.Stream, null, flags, note);
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, DateTime data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.DateTime, data, flags, note);
        return target;
    }

    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, TimeSpan data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.TimeSpan, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Vector2 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Vector2, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Vector3 data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Vector3, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Quaternion data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Quaternion, data, flags, note);
        return target;
    }
    
    public static SBTNodeDictionary Add(this SBTNodeDictionary target, string key, Color data, SBTFlags flags = SBTFlags.None, string note = null)
    {
        target.AddEntry(key, SBTType.Color, data, flags, note);
        return target;
    }
}