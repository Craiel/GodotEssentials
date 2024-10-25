namespace Craiel.Essentials.Events;

using Enums;
using Event;
using Godot;

public class UIEventShowFloatingText : IUIEvent
{
    public Vector2 Position = Vector2.Zero;
    public Color Color = Colors.White;
    public string Text = "#UNSET#";
    public TextFlags Flags = TextFlags.None;
}