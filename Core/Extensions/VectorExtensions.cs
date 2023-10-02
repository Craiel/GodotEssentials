namespace Craiel.Essentials.Extensions;

using Godot;

public static class VectorExtensions
{
    public static float[] ToArray(this Vector3 vector)
    {
        var result = new float[3];
        result[0] = vector.X;
        result[1] = vector.Y;
        result[2] = vector.Z;
        return result;
    }

    public static Vector3 Fill(float value)
    {
        return new Vector3(value, value, value);
    }

    public static Vector3 ToVector3(this Vector2I vector)
    {
        return new Vector3(vector.X, vector.Y, 0);
    }

    public static Vector3I ToVector3Int(this Vector2I vector)
    {
        return new Vector3I(vector.X, vector.Y, 0);
    }
}
