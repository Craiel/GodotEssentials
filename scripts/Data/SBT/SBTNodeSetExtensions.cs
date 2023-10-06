using System;
using Craiel.Essentials.Data.SBT.Nodes;
using Craiel.Essentials.Enums;
using Craiel.Essentials.Utils;
using Godot;

namespace Craiel.Essentials.Data.SBT;

public static class SBTNodeSetExtensions
{
    public static string ReadString(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeString>().Data;
    }
    
    public static bool ReadBool(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeBool>().Data;
    }
    
    public static byte ReadByte(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeByte>().Data;
    }
    
    public static short ReadShort(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeShort>().Data;
    }
    
    public static ushort ReadUShort(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeUShort>().Data;
    }
    
    public static int ReadInt(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeInt>().Data;
    }
    
    public static uint ReadUInt(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeUInt>().Data;
    }
    
    public static long ReadLong(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeLong>().Data;
    }
    
    public static ulong ReadULong(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeULong>().Data;
    }
    
    public static float ReadSingle(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeSingle>().Data;
    }
    
    public static double ReadDouble(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeDouble>().Data;
    }
    
    public static SBTFlags ReadFlags(this SBTNodeSet source)
    {
        return source.ReadNext().Flags;
    }
    
    public static string ReadNote(this SBTNodeSet source)
    {
        return source.ReadNext().Note;
    }
    
    public static SBTNodeArray<T> ReadArray<T>(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeArray<T>>();
    }
    
    public static SBTNodeList ReadList(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeList>();
    }
    
    public static SBTNodeDictionary ReadDictionary(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeDictionary>();
    }
    
    public static SBTNodeSet ReadSet(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeSet>();
    }
    
    public static SBTNodeStream ReadStream(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeStream>();
    }
    
    public static DateTime ReadDateTime(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeDateTime>().Data;
    }
    
    public static TimeSpan ReadTimeSpan(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeTimeSpan>().Data;
    }
    
    public static bool TryReadString(this SBTNodeSet source, out string result)
    {
        result = null;
        if (source.TryReadNext(out SBTNodeString node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static string TryReadString(this SBTNodeSet source, string defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeString node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadByte(this SBTNodeSet source, out byte result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeByte node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static byte TryReadByte(this SBTNodeSet source, byte defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeByte node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadShort(this SBTNodeSet source, out short result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static short TryReadShort(this SBTNodeSet source, short defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeShort node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUShort(this SBTNodeSet source, out ushort result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeUShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ushort TryReadUShort(this SBTNodeSet source, ushort defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeUShort node))
        {
            return node.Data;
        }

        return default;
    }
    
    public static bool TryReadInt(this SBTNodeSet source, out int result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }

    public static int TryReadInt(this SBTNodeSet source, int defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUInt(this SBTNodeSet source, out uint result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeUInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static uint TryReadUInt(this SBTNodeSet source, uint defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeUInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadLong(this SBTNodeSet source, out long result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeLong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static long TryReadLong(this SBTNodeSet source, long defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeLong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadULong(this SBTNodeSet source, out ulong result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeULong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ulong TryReadULong(this SBTNodeSet source, ulong defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeULong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadSingle(this SBTNodeSet source, out float result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeSingle node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static float TryReadSingle(this SBTNodeSet source, float defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeSingle node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadDouble(this SBTNodeSet source, out double result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeDouble node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static double TryReadDouble(this SBTNodeSet source, double defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeDouble node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadFlags(this SBTNodeSet source, out SBTFlags result)
    {
        result = default;
        if (source.TryReadNext(out ISBTNode node))
        {
            result = node.Flags;
            return true;
        }

        return false;
    }
    
    public static SBTFlags TryReadFlags(this SBTNodeSet source, SBTFlags defaultValue = default)
    {
        if (source.TryReadNext(out ISBTNode node))
        {
            return node.Flags;
        }

        return defaultValue;
    }
    
    public static bool TryReadArray<T>(this SBTNodeSet source, out SBTNodeArray<T> result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeArray<T> TryReadArray<T>(this SBTNodeSet source, SBTNodeArray<T> defaultValue = default)
    {
        return source.TryReadNext(out SBTNodeArray<T> result) ? result : defaultValue;
    }
    
    public static bool TryReadList(this SBTNodeSet source, out SBTNodeList result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeList TryReadList(this SBTNodeSet source, SBTNodeList defaultValue)
    {
        return source.TryReadNext(out SBTNodeList result) ? result : defaultValue;
    }
    
    public static bool TryReadDictionary(this SBTNodeSet source, out SBTNodeDictionary result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeDictionary TryReadDictionary(this SBTNodeSet source, SBTNodeDictionary defaultValue)
    {
        return source.TryReadNext(out SBTNodeDictionary result) ? result : defaultValue;
    }
    
    public static bool TryReadSet(this SBTNodeSet source, out SBTNodeSet result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeSet TryReadSet(this SBTNodeSet source, SBTNodeSet defaultValue)
    {
        return source.TryReadNext(out SBTNodeSet result) ? result : defaultValue;
    }
    
    public static bool TryReadStream(this SBTNodeSet source, out SBTNodeStream result)
    {
        return source.TryReadNext(out result);
    }
    
    public static SBTNodeStream TryReadStream(this SBTNodeSet source, SBTNodeStream defaultValue)
    {
        return source.TryReadNext(out SBTNodeStream result) ? result : defaultValue;
    }
    
    public static bool TryReadDateTime(this SBTNodeSet source, out DateTime result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeDateTime node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static DateTime TryReadDateTime(this SBTNodeSet source, DateTime defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeDateTime node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadTimeSpan(this SBTNodeSet source, out TimeSpan result)
    {
        result = default;
        if (source.TryReadNext(out SBTNodeTimeSpan node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static TimeSpan TryReadTimeSpan(this SBTNodeSet source, TimeSpan defaultValue = default)
    {
        if (source.TryReadNext(out SBTNodeTimeSpan node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static Vector2 ReadVector2(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeVector2>().Data;
    }
    
    public static Vector3 ReadVector3(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeVector3>().Data;
    }
    
    public static Quaternion ReadQuaternion(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeQuaternion>().Data;
    }
    
    public static Color ReadColor(this SBTNodeSet source)
    {
        return source.ReadNext<SBTNodeColor>().Data;
    }
}