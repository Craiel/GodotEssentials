namespace Craiel.Essentials.DB;

using System;

public readonly struct EnumGameDataId<T>
    where T: System.Enum
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public EnumGameDataId(T value, GameDataType type)
    {
        this.Value = value;
        this.Type = type;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly T Value;

    public readonly GameDataType Type;
    
    public static bool operator ==(EnumGameDataId<T> value1, EnumGameDataId<T> value2)
    {
        return value1.Equals(value2);
    }

    public static bool operator !=(EnumGameDataId<T> value1, EnumGameDataId<T> value2)
    {
        return !(value1 == value2);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is EnumGameDataId<T> && this.Equals((EnumGameDataId<T>)obj);
    }

    public bool Equals(EnumGameDataId<T> other)
    {
        return this.Value.Equals(other.Value) && this.Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Value, (int)this.Type);
    }

    public override string ToString()
    {
        return $"{this.Type}.{this.Value}";
    }
}