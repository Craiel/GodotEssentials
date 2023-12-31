namespace Craiel.Essentials.Data.SBT;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text;
using Enums;
using Extensions;
using Godot;
using IO;
using Nodes;
using Utils;

public static class SBTUtils
{
    private const int CompressionWarningThreshold = 256;

    public static readonly string SaveIndent = new String(' ', 4);

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public static ISBTNode GetNode(SBTType type, object data = null, SBTFlags flags = SBTFlags.None, string note = null)
    {
        if (type.IsSimpleType() && data == null)
        {
            throw new InvalidOperationException("Data must be set for simple types");
        }

        switch (type)
        {
            case SBTType.String:
            {
                return new SBTNodeString((string)data, flags, note);
            }

            case SBTType.Bool:
            {
                return new SBTNodeBool((bool)data, flags, note);
            }
            
            case SBTType.Byte:
            {
                return new SBTNodeByte((byte)data, flags, note);
            }

            case SBTType.Short:
            {
                return new SBTNodeShort((short)data, flags, note);
            }

            case SBTType.UShort:
            {
                return new SBTNodeUShort((ushort)data, flags, note);
            }

            case SBTType.Int:
            {
                return new SBTNodeInt((int)data, flags, note);
            }

            case SBTType.UInt:
            {
                return new SBTNodeUInt((uint)data, flags, note);
            }

            case SBTType.Long:
            {
                return new SBTNodeLong((long)data, flags, note);
            }

            case SBTType.ULong:
            {
                return new SBTNodeULong((ulong)data, flags, note);
            }

            case SBTType.Single:
            {
                return new SBTNodeSingle((float)data, flags, note);
            }

            case SBTType.Double:
            {
                return new SBTNodeDouble((double)data, flags, note);
            }

            case SBTType.StringArray:
            {
                return new SBTNodeArrayString(type, flags, note);
            }

            case SBTType.BoolArray:
            {
                return new SBTNodeArrayBool(type, flags, note);
            }
            
            case SBTType.ByteArray:
            {
                return new SBTNodeArrayByte(type, flags, note);
            }

            case SBTType.ShortArray:
            {
                return new SBTNodeArrayShort(type, flags, note);
            }

            case SBTType.UShortArray:
            {
                return new SBTNodeArrayUShort(type, flags, note);
            }

            case SBTType.IntArray:
            {
                return new SBTNodeArrayInt(type, flags, note);
            }

            case SBTType.UIntArray:
            {
                return new SBTNodeArrayUInt(type, flags, note);
            }

            case SBTType.LongArray:
            {
                return new SBTNodeArrayLong(type, flags, note);
            }

            case SBTType.ULongArray:
            {
                return new SBTNodeArrayULong(type, flags, note);
            }

            case SBTType.SingleArray:
            {
                return new SBTNodeArraySingle(type, flags, note);
            }

            case SBTType.DoubleArray:
            {
                return new SBTNodeArrayDouble(type, flags, note);
            }

            case SBTType.List:
            {
                return new SBTNodeList(flags, note);
            }

            case SBTType.Dictionary:
            {
                return new SBTNodeDictionary(flags, note);
            }

            case SBTType.Set:
            {
                return new SBTNodeSet(flags, note);
            }

            case SBTType.Stream:
            {
                return new SBTNodeStream(flags, note);
            }

            case SBTType.Vector2:
            {
                return new SBTNodeVector2((Vector2)data, flags, note);
            }

            case SBTType.Vector3:
            {
                return new SBTNodeVector3((Vector3)data, flags, note);
            }

            case SBTType.Quaternion:
            {
                return new SBTNodeQuaternion((Quaternion)data, flags, note);
            }

            case SBTType.Color:
            {
                return new SBTNodeColor((Color)data, flags, note);
            }

            case SBTType.DateTime:
            {
                return new SBTNodeDateTime((DateTime) data, flags, note);
            }

            case SBTType.TimeSpan:
            {
                return new SBTNodeTimeSpan((TimeSpan) data, flags, note);
            }

            default:
            {
                throw new InvalidDataException("Invalid Binary: " + type);
            }
        }
    }

    public static string SerializeToString(this ISBTNode node)
    {
        byte[] data = node.Serialize();
        return Convert.ToBase64String(data);
    }

    public static byte[] Serialize(this ISBTNode node)
    {
        using (var stream = new MemoryStream())
        {
            node.Serialize(stream);

            byte[] data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            return data;
        }
    }

    public static byte[] SerializeCompressed(this ISBTNode node)
    {
        using (var stream = new MemoryStream())
        {
            byte[] rawData = Serialize(node);
            if (rawData.Length <= CompressionWarningThreshold)
            {
                EssentialCore.Logger.Warn("SBT Compressed Serialize called on small data, use compression on large data sets only");
            }

            using(var zipStream = new GZipStream(stream, CompressionLevel.Optimal, true))
            {
                zipStream.Write(rawData, 0, rawData.Length);
                zipStream.Flush();
            }

            byte[] data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            return data;
        }
    }

    public static void SerializeToFile(this ISBTNode node, ManagedFile target)
    {
        target.GetDirectory().Create();
        using (var stream = target.OpenWrite())
        {
            byte[] data = node.Serialize();
            stream.Write(data, 0, data.Length);
        }
    }

    public static void SerializeToFileCompressed(this ISBTNode node, ManagedFile target)
    {
        target.GetDirectory().Create();
        using (var stream = target.OpenWrite())
        {
            byte[] data = node.SerializeCompressed();
            stream.Write(data, 0, data.Length);
        }
    }

    public static void Serialize(this ISBTNode node, Stream target)
    {
        using (var writer = new BinaryWriter(target, Encoding.UTF8, true))
        {
            node.WriteHeader(writer);
            node.Save(writer);
        }
    }

    public static void Serialize(this ISBTNode node, ISBTNodeSerializer target)
    {
        target.Serialize(node);
    }

    public static ISBTNode Deserialize(string data)
    {
        return Deserialize(Convert.FromBase64String(data));
    }

    public static ISBTNode Deserialize(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
            return Deserialize(stream);
        }
    }

    public static ISBTNode DeserializeCompressed(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
            using (var decompressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
                {
                    zipStream.CopyTo(decompressedStream);
                }

                decompressedStream.Seek(0, SeekOrigin.Begin);
                return Deserialize(decompressedStream);
            }
        }
    }

    public static ISBTNode DeserializeCompressed(Stream stream)
    {
        using (var decompressedStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                zipStream.CopyTo(decompressedStream);
            }

            decompressedStream.Seek(0, SeekOrigin.Begin);
            return Deserialize(decompressedStream);
        }
    }

    public static ISBTNode Deserialize(Stream stream)
    {
        using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
        {
            SBTType type;
            SBTFlags flags;
            ReadHeader(reader, out type, out flags);

            switch (type)
            {
                case SBTType.Stream:
                {
                    var result = new SBTStream();
                    result.Load(reader);
                    return result;
                }
                    
                case SBTType.Set:
                {
                    var result = new SBTSet();
                    result.Load(reader);
                    return result;
                }
                
                case SBTType.List:
                {
                    var result = new SBTList();
                    result.Load(reader);
                    return result;
                }

                case SBTType.Dictionary:
                {
                    var result = new SBTDictionary();
                    result.Load(reader);
                    return result;
                }

                default:
                {
                    throw new InvalidDataException("SBT had unexpected root type: " + type);
                }
            }
        }
    }

    internal static void WriteHeader(this ISBTNode node, BinaryWriter target)
    {
        target.Write((byte)node.Type);
        target.Write((ushort)node.Flags);
    }

    internal static void ReadHeader(BinaryReader source, out SBTType nodeType, out SBTFlags nodeFlags)
    {
        nodeType = (SBTType)source.ReadByte();
        nodeFlags = (SBTFlags)source.ReadUInt16();
    }

    public static object ReadSimpleTypeData(SBTType type, BinaryReader reader)
    {
        switch (type)
        {
            case SBTType.String:
            {
                return reader.ReadString();
            }

            case SBTType.Bool:
            {
                return reader.ReadBoolean();
            }
            
            case SBTType.Byte:
            {
                return reader.ReadByte();
            }

            case SBTType.Short:
            {
                return reader.ReadInt16();
            }

            case SBTType.UShort:
            {
                return reader.ReadUInt16();
            }

            case SBTType.Int:
            {
                return reader.ReadInt32();
            }

            case SBTType.UInt:
            {
                return reader.ReadUInt32();
            }

            case SBTType.Long:
            {
                return reader.ReadInt64();
            }

            case SBTType.ULong:
            {
                return reader.ReadUInt64();
            }

            case SBTType.Single:
            {
                return reader.ReadSingle();
            }

            case SBTType.Double:
            {
                return reader.ReadDouble();
            }

            case SBTType.Vector2:
            {
                return reader.ReadVector2();
            }

            case SBTType.Vector3:
            {
                return reader.ReadVector3();
            }

            case SBTType.Quaternion:
            {
                return reader.ReadQuaternion();
            }

            case SBTType.Color:
            {
                return reader.ReadColor();
            }

            case SBTType.DateTime:
            {
                return reader.ReadDateTime();
            }

            case SBTType.TimeSpan:
            {
                return reader.ReadTimeSpan();
            }

            default:
            {
                throw new InvalidOperationException("Invalid SimpleType: " + type);
            }
        }
    }

    public static byte? GetDataEntrySize(SBTType type)
    {
        switch (type)
        {
            case SBTType.Bool:
            {
                return sizeof(bool);
            }
            
            case SBTType.Byte:
            {
                return sizeof(byte);
            }

            case SBTType.Short:
            {
                return sizeof(short);
            }

            case SBTType.UShort:
            {
                return sizeof(ushort);
            }

            case SBTType.Int:
            {
                return sizeof(int);
            }

            case SBTType.UInt:
            {
                return sizeof(uint);
            }

            case SBTType.Long:
            {
                return sizeof(long);
            }

            case SBTType.ULong:
            {
                return sizeof(ulong);
            }

            case SBTType.Single:
            {
                return sizeof(float);
            }

            case SBTType.Double:
            {
                return sizeof(double);
            }

            case SBTType.Vector2:
            {
                return sizeof(float) * 2;
            }

            case SBTType.Vector3:
            {
                return sizeof(float) * 3;
            }

            case SBTType.Quaternion:
            {
                return sizeof(float) * 4;
            }

            case SBTType.Color:
            {
                return sizeof(float) * 4;
            }

            case SBTType.DateTime:
            case SBTType.TimeSpan:
            {
                return sizeof(long);
            }

            default:
            {
                return null;
            }
        }
    }

    public static SBTType GetArrayBaseType(Type type)
    {
        if (type == TypeDef<string>.Value)
        {
            return SBTType.StringArray;
        }

        if (type == TypeDef<byte>.Value)
        {
            return SBTType.ByteArray;
        }

        if (type == TypeDef<short>.Value)
        {
            return SBTType.ShortArray;
        }

        if (type == TypeDef<ushort>.Value)
        {
            return SBTType.UShortArray;
        }

        if (type == TypeDef<int>.Value)
        {
            return SBTType.IntArray;
        }

        if (type == TypeDef<uint>.Value)
        {
            return SBTType.UIntArray;
        }

        if (type == TypeDef<long>.Value)
        {
            return SBTType.LongArray;
        }

        if (type == TypeDef<ulong>.Value)
        {
            return SBTType.ULongArray;
        }

        if (type == TypeDef<float>.Value)
        {
            return SBTType.SingleArray;
        }

        if (type == TypeDef<double>.Value)
        {
            return SBTType.DoubleArray;
        }

        throw new ArgumentException("Type not supported for SBT Array");
    }
}