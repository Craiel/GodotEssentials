namespace Craiel.Essentials.DB;

using System;
using System.Diagnostics;
using SaveLoad;
using Godot.Collections;

[DebuggerDisplay("{Type}.{Value}")]
public struct StringGameDataId : IGameDataId, ISaveLoadDataBlock
{
    const string UnsetIdValue = "__INTERNAL__UNSET__";

    public static readonly StringGameDataId Unset = new(UnsetIdValue, GameDataType.Unset);

    private string value;
    private GameDataType dataType;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public StringGameDataId(string id, GameDataType type)
    {
        this.value = id;
        this.dataType = type;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public readonly string Value => this.value;
    public GameDataType Type => this.dataType;
    
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
        return this.value == other.Value && this.dataType == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.value, (int)this.dataType);
    }

    public override string ToString()
    {
        return $"{this.dataType}.{this.value}";
    }

    public void SaveTo(Dictionary target)
    {
        target["sgdi_v"] = this.value;
        target["sgdi_t"] = (int)this.dataType;
    }

    public void LoadFrom(Dictionary source)
    {
        this.value = source["sgdi_v"].AsString();
        this.dataType = (GameDataType)source["sgdi_t"].AsInt32();
    }
}