using System;
using Craiel.Essentials.Data.SBT.Nodes;
using Craiel.Essentials.Enums;
using Craiel.Essentials.Utils;
using Godot;

namespace Craiel.Essentials.Data.SBT;

public static class SBTNodeStreamExtensions
{
    public static string ReadString(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeString>().Data;
    }
    
    public static bool ReadBool(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeBool>().Data;
    }
    
    public static byte ReadByte(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeByte>().Data;
    }
    
    public static short ReadShort(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeShort>().Data;
    }
    
    public static ushort ReadUShort(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeUShort>().Data;
    }
    
    public static int ReadInt(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeInt>().Data;
    }
    
    public static uint ReadUInt(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeUInt>().Data;
    }
    
    public static long ReadLong(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeLong>().Data;
    }
    
    public static ulong ReadULong(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeULong>().Data;
    }
    
    public static float ReadSingle(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeSingle>().Data;
    }
    
    public static double ReadDouble(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeDouble>().Data;
    }
    
    public static SBTFlags ReadFlags(this SBTNodeStream source, int index)
    {
        return source.ReadNext().Flags;
    }
    
    public static string ReadNote(this SBTNodeStream source, int index)
    {
        return source.ReadNext().Note;
    }
    
    public static SBTNodeArray<T> ReadArray<T>(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeArray<T>>();
    }
    
    public static SBTNodeList ReadList(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeList>();
    }
    
    public static SBTNodeDictionary ReadDictionary(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeDictionary>();
    }
    
    public static SBTNodeStream ReadStream(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeStream>();
    }
    
    public static DateTime ReadDateTime(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeDateTime>().Data;
    }
    
    public static TimeSpan ReadTimeSpan(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeTimeSpan>().Data;
    }
    
    public static bool TryReadString(this SBTNodeStream source, int index, out string result)
    {
        result = null;
        if (source.TryReadNext(out SBTNodeString node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static string TryReadString(this SBTNodeStream source, int index, string defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeString node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadByte(this SBTNodeStream source, int index, out byte result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeByte node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static byte TryReadByte(this SBTNodeStream source, int index, byte defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeByte node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadShort(this SBTNodeStream source, int index, out short result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static short TryReadShort(this SBTNodeStream source, int index, short defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeShort node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUShort(this SBTNodeStream source, int index, out ushort result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeUShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ushort TryReadUShort(this SBTNodeStream source, int index, ushort defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeUShort node))
        {
            return node.Data;
        }

        return default;
    }
    
    public static bool TryReadInt(this SBTNodeStream source, int index, out int result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }

    public static int TryReadInt(this SBTNodeStream source, int index, int defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUInt(this SBTNodeStream source, int index, out uint result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeUInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static uint TryReadUInt(this SBTNodeStream source, int index, uint defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeUInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadLong(this SBTNodeStream source, int index, out long result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeLong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static long TryReadLong(this SBTNodeStream source, int index, long defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeLong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadULong(this SBTNodeStream source, int index, out ulong result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeULong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ulong TryReadULong(this SBTNodeStream source, int index, ulong defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeULong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadSingle(this SBTNodeStream source, int index, out float result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeSingle node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static float TryReadSingle(this SBTNodeStream source, int index, float defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeSingle node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadDouble(this SBTNodeStream source, int index, out double result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeDouble node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static double TryReadDouble(this SBTNodeStream source, int index, double defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeDouble node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadFlags(this SBTNodeStream source, int index, out SBTFlags result)
    {
        result = default;
        if (source.TryReadNext(out ISBTNode node))
        {
            result = node.Flags;
            return true;
        }

        return false;
    }
    
    public static SBTFlags TryReadFlags(this SBTNodeStream source, int index, SBTFlags defaultValue = default)
    {
        if (source.TryReadNext(out ISBTNode node))
        {
            return node.Flags;
        }

        return defaultValue;
    }
    
    public static bool TryReadArray<T>(this SBTNodeStream source, int index, out SBTNodeArray<T> result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeArray<T> TryReadArray<T>(this SBTNodeStream source, int index, SBTNodeArray<T> defaultValue = default)
    {
        return source.TryReadNext(out SBTNodeArray<T> result) ? result : defaultValue;
    }
    
    public static bool TryReadList(this SBTNodeStream source, int index, out SBTNodeList result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeList TryReadList(this SBTNodeStream source, int index, SBTNodeList defaultValue)
    {
        return source.TryReadNext(out SBTNodeList result) ? result : defaultValue;
    }
    
    public static bool TryReadDictionary(this SBTNodeStream source, int index, out SBTNodeDictionary result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeDictionary TryReadDictionary(this SBTNodeStream source, int index, SBTNodeDictionary defaultValue)
    {
        return source.TryReadNext(out SBTNodeDictionary result) ? result : defaultValue;
    }
    
    public static bool TryReadStream(this SBTNodeStream source, int index, out SBTNodeStream result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeStream TryReadStream(this SBTNodeStream source, int index, SBTNodeStream defaultValue)
    {
        return source.TryReadNext(out SBTNodeStream result) ? result : defaultValue;
    }
    
    public static bool TryReadDateTime(this SBTNodeStream source, int index, out DateTime result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeDateTime node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static DateTime TryReadDateTime(this SBTNodeStream source, int index, DateTime defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeDateTime node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadTimeSpan(this SBTNodeStream source, int index, out TimeSpan result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeTimeSpan node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static TimeSpan TryReadTimeSpan(this SBTNodeStream source, int index, TimeSpan defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeTimeSpan node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static Vector2 ReadVector2(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeVector2>().Data;
    }
    
    public static Vector3 ReadVector3(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeVector3>().Data;
    }
    
    public static Quaternion ReadQuaternion(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeQuaternion>().Data;
    }
    
    public static Color ReadColor(this SBTNodeStream source, int index)
    {
        return source.ReadNext<SBTNodeColor>().Data;
    }
}