namespace Craiel.Essentials.Utils;

using System;

public static class EnumDefLong<T>
    where T : Enum, IConvertible
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    static EnumDefLong()
    {
        Type = TypeDef<T>.Value;
        Values = (T[])Enum.GetValues(Type);
        Names = Enum.GetNames(Type);
        
        Count = Values.Length;

        MinValue = long.MaxValue;
        MaxValue = long.MinValue;
        for (var i = 0; i < Count; i++)
        {
            long value = (long)(object)Values[i];
            if(value < MinValue)
            {
                MinValue = value;
            }

            if (value > MaxValue)
            {
                MaxValue = value;
            }
        }
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static readonly Type Type;

    public static readonly long Count;
    public static readonly long MinValue;
    public static readonly long MaxValue;

    public static readonly T[] Values;
    public static readonly string[] Names;

    public static T Parse(string value)
    {
        return (T)Enum.Parse(Type, value);
    }
}