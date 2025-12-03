namespace Craiel.Essentials.SaveLoad;

using Godot.Collections;

public interface ISaveLoadDataBlock
{
    void SaveTo(Dictionary target, string prefix);
    void LoadFrom(Dictionary source, string prefix);
}
