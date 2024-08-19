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

        MinValue = int.MaxValue;
        MaxValue = int.MinValue;
        for (var i = 0; i < Count; i++)
        {
            int value = (int)(object)Values[i];
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

    public static readonly int Count;
    public static readonly int MinValue;
    public static readonly int MaxValue;

    public static readonly T[] Values;
    public static readonly string[] Names;

    public static T Parse(string value)
    {
        return (T)Enum.Parse(Type, value);
    }
}