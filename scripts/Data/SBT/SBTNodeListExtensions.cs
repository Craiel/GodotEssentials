using System;
using Craiel.Essentials.Data.SBT.Nodes;
using Craiel.Essentials.Enums;
using Craiel.Essentials.Utils;
using Godot;

namespace Craiel.Essentials.Data.SBT;

public static class SBTNodeListExtensions
{
    public static string ReadString(this SBTList source, int index)
    {
        return source.Read<SBTNodeString>(index).Data;
    }
    
    public static bool ReadBool(this SBTList source, int index)
    {
        return source.Read<SBTNodeBool>(index).Data;
    }
    
    public static byte ReadByte(this SBTList source, int index)
    {
        return source.Read<SBTNodeByte>(index).Data;
    }
    
    public static short ReadShort(this SBTList source, int index)
    {
        return source.Read<SBTNodeShort>(index).Data;
    }
    
    public static ushort ReadUShort(this SBTList source, int index)
    {
        return source.Read<SBTNodeUShort>(index).Data;
    }
    
    public static int ReadInt(this SBTList source, int index)
    {
        return source.Read<SBTNodeInt>(index).Data;
    }
    
    public static uint ReadUInt(this SBTList source, int index)
    {
        return source.Read<SBTNodeUInt>(index).Data;
    }
    
    public static long ReadLong(this SBTList source, int index)
    {
        return source.Read<SBTNodeLong>(index).Data;
    }
    
    public static ulong ReadULong(this SBTList source, int index)
    {
        return source.Read<SBTNodeULong>(index).Data;
    }
    
    public static float ReadSingle(this SBTList source, int index)
    {
        return source.Read<SBTNodeSingle>(index).Data;
    }
    
    public static double ReadDouble(this SBTList source, int index)
    {
        return source.Read<SBTNodeDouble>(index).Data;
    }
    
    public static SBTFlags ReadFlags(this SBTList source, int index)
    {
        return source.Read(index).Flags;
    }
    
    public static string ReadNote(this SBTList source, int index)
    {
        return source.Read(index).Note;
    }
    
    public static SBTNodeArray<T> ReadArray<T>(this SBTList source, int index)
    {
        return source.Read<SBTNodeArray<T>>(index);
    }
    
    public static SBTNodeList ReadList(this SBTList source, int index)
    {
        return source.Read<SBTNodeList>(index);
    }
    
    public static SBTNodeDictionary ReadDictionary(this SBTList source, int index)
    {
        return source.Read<SBTNodeDictionary>(index);
    }
    
    public static SBTNodeStream ReadStream(this SBTList source, int index)
    {
        return source.Read<SBTNodeStream>(index);
    }
    
    public static DateTime ReadDateTime(this SBTList source, int index)
    {
        return source.Read<SBTNodeDateTime>(index).Data;
    }
    
    public static TimeSpan ReadTimeSpan(this SBTList source, int index)
    {
        return source.Read<SBTNodeTimeSpan>(index).Data;
    }
    
    public static bool TryReadString(this SBTList source, int index, out string result)
    {
        result = null;
        if (source.TryRead(index, out SBTNodeString node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static string TryReadString(this SBTList source, int index, string defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeString node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadByte(this SBTList source, int index, out byte result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeByte node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static byte TryReadByte(this SBTList source, int index, byte defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeByte node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadShort(this SBTList source, int index, out short result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static short TryReadShort(this SBTList source, int index, short defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeShort node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUShort(this SBTList source, int index, out ushort result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeUShort node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ushort TryReadUShort(this SBTList source, int index, ushort defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeUShort node))
        {
            return node.Data;
        }

        return default;
    }
    
    public static bool TryReadInt(this SBTList source, int index, out int result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }

    public static int TryReadInt(this SBTList source, int index, int defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadUInt(this SBTList source, int index, out uint result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeUInt node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static uint TryReadUInt(this SBTList source, int index, uint defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeUInt node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadLong(this SBTList source, int index, out long result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeLong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static long TryReadLong(this SBTList source, int index, long defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeLong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadULong(this SBTList source, int index, out ulong result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeULong node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static ulong TryReadULong(this SBTList source, int index, ulong defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeULong node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadSingle(this SBTList source, int index, out float result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeSingle node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static float TryReadSingle(this SBTList source, int index, float defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeSingle node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadDouble(this SBTList source, int index, out double result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeDouble node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static double TryReadDouble(this SBTList source, int index, double defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeDouble node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadFlags(this SBTList source, int index, out SBTFlags result)
    {
        result = default;
        if (source.TryRead(index, out ISBTNode node))
        {
            result = node.Flags;
            return true;
        }

        return false;
    }
    
    public static SBTFlags TryReadFlags(this SBTList source, int index, SBTFlags defaultValue = default)
    {
        if (source.TryRead(index, out ISBTNode node))
        {
            return node.Flags;
        }

        return defaultValue;
    }
    
    public static bool TryReadArray<T>(this SBTList source, int index, out SBTNodeArray<T> result)
    {
        return source.TryRead(index, out result);
    }
    
    public static SBTNodeArray<T> TryReadArray<T>(this SBTList source, int index, SBTNodeArray<T> defaultValue = default)
    {
        return source.TryRead(index, out SBTNodeArray<T> result) ? result : defaultValue;
    }
    
    public static bool TryReadList(this SBTList source, int index, out SBTNodeList result)
    {
        return source.TryRead(index, out result);
    }
    
    public static SBTNodeList TryReadList(this SBTList source, int index, SBTNodeList defaultValue)
    {
        return source.TryRead(index, out SBTNodeList result) ? result : defaultValue;
    }
    
    public static bool TryReadDictionary(this SBTList source, int index, out SBTNodeDictionary result)
    {
        return source.TryRead(index, out result);
    }
    
    public static SBTNodeDictionary TryReadDictionary(this SBTList source, int index, SBTNodeDictionary defaultValue)
    {
        return source.TryRead(index, out SBTNodeDictionary result) ? result : defaultValue;
    }
    
    public static bool TryReadStream(this SBTList source, int index, out SBTNodeStream result)
    {
        return source.TryRead(index, out result);
    }
    
    public static SBTNodeStream TryReadStream(this SBTList source, int index, SBTNodeStream defaultValue)
    {
        return source.TryRead(index, out SBTNodeStream result) ? result : defaultValue;
    }
    
    public static bool TryReadDateTime(this SBTList source, int index, out DateTime result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeDateTime node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static DateTime TryReadDateTime(this SBTList source, int index, DateTime defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeDateTime node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static bool TryReadTimeSpan(this SBTList source, int index, out TimeSpan result)
    {
        result = default;
        if (source.TryRead(index, out SBTNodeTimeSpan node))
        {
            result = node.Data;
            return true;
        }

        return false;
    }
    
    public static TimeSpan TryReadTimeSpan(this SBTList source, int index, TimeSpan defaultValue = default)
    {
        if (source.TryRead(index, out SBTNodeTimeSpan node))
        {
            return node.Data;
        }

        return defaultValue;
    }
    
    public static Vector2 ReadVector2(this SBTList source, int index)
    {
        return source.Read<SBTNodeVector2>(index).Data;
    }
    
    public static Vector3 ReadVector3(this SBTList source, int index)
    {
        return source.Read<SBTNodeVector3>(index).Data;
    }
    
    public static Quaternion ReadQuaternion(this SBTList source, int index)
    {
        return source.Read<SBTNodeQuaternion>(index).Data;
    }
    
    public static Color ReadColor(this SBTList source, int index)
    {
        return source.Read<SBTNodeColor>(index).Data;
    }
}