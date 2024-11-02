namespace Craiel.Essentials.Input;

using Godot;

public struct InputMappingInfo
{
    public InputDeviceType Device;
    public InputMappingType Type;
    public Key Key;
    public JoyButton JoyButton;
    public JoyAxis Axis;
    public float AxisSign;
}