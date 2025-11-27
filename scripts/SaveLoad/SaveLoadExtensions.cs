namespace Craiel.Essentials.SaveLoad;

using DB;
using Godot.Collections;

public static class SaveLoadExtensions
{
    private const string StringGameDataIdType = "_sgdi_t";
    private const string StringGameDataIdValue = "_sgdi_v";

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void SetStringGameDataId(this Dictionary target, StringGameDataId id, string prefix = "")
    {
        target[prefix + StringGameDataIdType] = (int)id.Type;
        target[prefix + StringGameDataIdValue] = id.Value;
    }

    public static StringGameDataId GetStringGameDataId(this Dictionary source, string prefix = "")
    {
        var type = (GameDataType)source[prefix + StringGameDataIdType].AsInt32();
        var value = source[prefix + StringGameDataIdValue].AsString();
        return new StringGameDataId(value, type);
    }
}
