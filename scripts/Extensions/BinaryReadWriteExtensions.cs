namespace Craiel.Essentials.Extensions;

using System;
using System.IO;
using Godot;

public static class BinaryReadWriteExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void Write(this BinaryWriter writer, Aabb bounds)
    {
        writer.Write(bounds.Position);
        writer.Write(bounds.Size);
    }

    public static void Write(this BinaryWriter writer, Vector3 vector)
    {
        writer.Write(vector.X);
        writer.Write(vector.Y);
        writer.Write(vector.Z);
    }

    public static void Write(this BinaryWriter writer, Vector2 vector)
    {
        writer.Write(vector.X);
        writer.Write(vector.Y);
    }
    
    public static void Write(this BinaryWriter writer, Quaternion quaternion)
    {
        writer.Write(quaternion.X);
        writer.Write(quaternion.Y);
        writer.Write(quaternion.Z);
        writer.Write(quaternion.W);
    }

    public static void Write(this BinaryWriter writer, Color color)
    {
        writer.Write(color.R);
        writer.Write(color.G);
        writer.Write(color.B);
        writer.Write(color.A);
    }

    public static void Write(this BinaryWriter writer, DateTime time)
    {
        writer.Write(time.Ticks);
    }

    public static void Write(this BinaryWriter writer, TimeSpan timeSpan)
    {
        writer.Write(timeSpan.Ticks);
    }
    
    public static Vector3 ReadVector3(this BinaryReader reader)
    {
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();
        return new Vector3(x, y, z);
    }

    public static Vector2 ReadVector2(this BinaryReader reader)
    {
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        return new Vector2(x, y);
    }

    public static Quaternion ReadQuaternion(this BinaryReader reader)
    {
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();
        float w = reader.ReadSingle();
        return new Quaternion(x, y, z, w);
    }

    public static Aabb ReadBoundingBox(this BinaryReader reader)
    {
        Vector3 position = reader.ReadVector3();
        Vector3 size = reader.ReadVector3();

        return new Aabb(position, size);
    }

    public static Color ReadColor(this BinaryReader reader)
    {
        float r = reader.ReadSingle();
        float g = reader.ReadSingle();
        float b = reader.ReadSingle();
        float a = reader.ReadSingle();
        return new Color(r, g, b, a);
    }

    public static DateTime ReadDateTime(this BinaryReader reader)
    {
        return new DateTime(reader.ReadInt64());
    }

    public static TimeSpan ReadTimeSpan(this BinaryReader reader)
    {
        return new TimeSpan(reader.ReadInt64());
    }
}
