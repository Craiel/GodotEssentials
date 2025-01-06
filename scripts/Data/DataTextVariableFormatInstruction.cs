namespace Craiel.Essentials.Data;

using Godot;

public struct DataTextVariableFormatInstruction
{
    public DataTextVariableFormatInstruction(string name, string value)
    {
        this.Name = name;
        this.Value = value;
        this.Type = DataTextVariableFormatType.Plain;
        this.Color = Colors.White;
    }
    
    public string Name;
    public string Value;
    public DataTextVariableFormatType Type;
    public Color Color;
}