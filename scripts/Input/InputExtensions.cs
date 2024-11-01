namespace Craiel.Essentials.Input;

using Godot;

public static class InputExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static InputEvent GetEvent(this InputMappingInfo info)
    {
        switch (info.Type)
        {
            case InputMappingType.Key:
            {
                return new InputEventKey { PhysicalKeycode = info.Key };
            }

            case InputMappingType.JoyButton:
            {
                return new InputEventJoypadButton { ButtonIndex = info.JoyButton };
            }

            case InputMappingType.JoyMotion:
            {
                return new InputEventJoypadMotion { Axis = info.Axis, AxisValue = info.AxisSign };
            }
        }

        return null;
    }
    
    public static bool GetInfo(this InputEvent eventData, out InputMappingInfo info)
    {
        if (eventData is InputEventKey key)
        {
            info = new InputMappingInfo
            {
                Type = InputMappingType.Key,
                Key = key.PhysicalKeycode
            };

            return true;
        }

        if (eventData is InputEventJoypadButton joyButton)
        {
            info = new InputMappingInfo
            {
                Type = InputMappingType.JoyButton,
                JoyButton = joyButton.ButtonIndex
            };

            return true;
        }

        if (eventData is InputEventJoypadMotion joyMotion)
        {
            info = new InputMappingInfo
            {
                Type = InputMappingType.JoyMotion,
                Axis = joyMotion.Axis,
                AxisSign = Mathf.Sign(joyMotion.AxisValue)
            };

            return true;
        }

        info = default;
        return false;
    }
    
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