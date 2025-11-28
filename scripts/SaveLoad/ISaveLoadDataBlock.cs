namespace Craiel.Essentials.SaveLoad;

using Godot.Collections;

public interface ISaveLoadDataBlock
{
    void SaveTo(Dictionary target);
    void LoadFrom(Dictionary source);
}
