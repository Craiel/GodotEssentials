namespace Craiel.Essentials.GameData;

using System;
using System.ComponentModel;

[TypeConverter(typeof(GameDataIdTypeConverter))]
[Serializable]
public struct GameDataId
{
    public const uint InvalidId = 0;
    public const uint FirstValidId = 1;

    public static readonly GameDataId Invalid = new GameDataId();

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public GameDataId(uint id)
    {
        this.Value = id;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public uint Value;

    public static bool operator ==(GameDataId value1, GameDataId value2)
    {
        return value1.Equals(value2);
    }

    public static bool operator !=(GameDataId value1, GameDataId value2)
    {
        return !(value1 == value2);
    }

    public bool Equals(GameDataId other)
    {
        bool noneHasId = this.Value == InvalidId && other.Value == InvalidId;
        bool bothHaveId = this.Value != InvalidId && other.Value != InvalidId;

        if (bothHaveId)
        {
            // Easy case, simple id compare
            return (this.Value != InvalidId && other.Value != InvalidId) && this.Value == other.Value;
        }

        // At this point they can only be equal if they have nothing
        return noneHasId;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is GameDataId && this.Equals((GameDataId)obj);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }
}
