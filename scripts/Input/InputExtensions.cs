namespace Craiel.Essentials.Input;

using Godot;

public static class InputExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static bool IsControllerEvent(this InputEvent eventData)
    {
        if (eventData is InputEventJoypadButton buttonEvent)
        {
            return true;
        }

        if (eventData is InputEventJoypadMotion motionEvent)
        {
            return true;
        }

        return false;
    }

    public static bool IsKeyboardEvent(this InputEvent eventData)
    {
        if (eventData is InputEventKey keyEvent)
        {
            return true;
        }

        return false;
    }
}