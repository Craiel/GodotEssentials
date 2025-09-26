namespace Craiel.Essentials.DB;

using System;
using System.Diagnostics;

[DebuggerDisplay("{Type}.{Value}")]
public readonly struct StringGameDataId : IGameDataId
{
    const string UnsetIdValue = "__INTERNAL__UNSET__";
    
    public static readonly StringGameDataId Unset = new(UnsetIdValue, GameDataType.Unset);

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public StringGameDataId(string id, GameDataType type)
    {
        this.Value = id;
        this.Type = type;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly string Value;

    public GameDataType Type { get; }
    
    public GameDataIdType IDType => GameDataIdType.String;

    public static bool operator ==(StringGameDataId value1, StringGameDataId value2)
    {
        return value1.Equals(value2);
    }

    public static bool operator !=(StringGameDataId value1, StringGameDataId value2)
    {
        return !(value1 == value2);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is StringGameDataId && this.Equals((StringGameDataId)obj);
    }

    public bool Equals(StringGameDataId other)
    {
        return this.Value == other.Value && this.Type == other.Type;
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