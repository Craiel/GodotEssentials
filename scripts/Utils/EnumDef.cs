namespace Craiel.Essentials.Utils;

using System;

public static class EnumDef<T>
    where T : Enum, IConvertible
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    static EnumDef()
    {
        Type = TypeDef<T>.Value;
        Values = (T[])Enum.GetValues(Type);
        Names = Enum.GetNames(Type);

        Count = Values.Length;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static readonly Type Type;

    public static readonly int Count;

    public static readonly T[] Values;
    public static readonly string[] Names;
}