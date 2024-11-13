namespace Craiel.Essentials.Settings;

using System;
using Godot;
using Input;
using Utils;

public static class GameSettingsInput
{
    private const string InputInfoDeviceKey = "input_info_device_";
    private const string InputInfoTypeKey = "input_info_type_";
    private const string InputInfoKeyKey = "input_info_key_";
    private const string InputInfoJoyButtonKey = "input_info_joy_button_";
    private const string InputInfoJoyAxisKey = "input_info_joy_axis_";
    private const string InputInfoAxisSignKey = "input_info_axis_sign_";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void ResetToDefault<T>(InputDeviceType type)
        where T: Enum
    {
        // Erase all overrides
        switch (type)
        {
            case InputDeviceType.Keyboard:
            {
                GameSettings.Erase(GameSettingsSection.Keyboard);
                break;
            }

            case InputDeviceType.Controller:
            {
                GameSettings.Erase(GameSettingsSection.Gamepad);
                break;
            }

            default:
            {
                return;
            }
        }
        
        GameSettings.Save();
        Apply<T>(type);
    }
    
    public static void SetOverride(string action, InputMappingInfo info, bool save = true)
    {
        InputEvent currentEvent = GetCurrentlyMappedEvent(action, info.Device);
        if (currentEvent != null)
        {
            InputMap.ActionEraseEvent(action, currentEvent);
        }

        var input = info.GetEvent();
        if (input != null)
        {
            InputMap.ActionAddEvent(action, input);

            if (save)
            {
                GameSettingsSection saveSection = info.Type == InputMappingType.Key
                    ? GameSettingsSection.Keyboard
                    : GameSettingsSection.Gamepad;

                GameSettings.Set(saveSection, InputInfoDeviceKey + action, (ushort)info.Device);
                GameSettings.Set(saveSection, InputInfoTypeKey + action, (ushort)info.Type);
                GameSettings.Set(saveSection, InputInfoKeyKey + action, (long)info.Key);
                GameSettings.Set(saveSection, InputInfoJoyButtonKey + action, (long)info.JoyButton);
                GameSettings.Set(saveSection, InputInfoJoyAxisKey + action, (long)info.Axis);
                GameSettings.Set(saveSection, InputInfoAxisSignKey + action, info.AxisSign);
            }
        }
    }
    
    public static void Apply<T>(InputDeviceType type)
        where T: Enum
    {
        GameSettingsSection saveSection = type == InputDeviceType.Keyboard
            ? GameSettingsSection.Keyboard
            : GameSettingsSection.Gamepad;
        
        // Restore default first, then re-apply what we have
        InputController.RestoreDefaultMappings(type);
        foreach (T action in EnumDefInt<T>.Values)
        {
            string actionString = action.ToString();
            ushort deviceValue = GameSettings.Get(saveSection, InputInfoDeviceKey + actionString, ushort.MaxValue).AsUInt16();
            if (deviceValue == ushort.MaxValue)
            {
                continue;
            }

            var mapping = new InputMappingInfo
            {
                Device = (InputDeviceType)deviceValue,
                Type = (InputMappingType)GameSettings.Get(saveSection, InputInfoTypeKey + actionString, 0).AsUInt16(),
                Key = (Key)GameSettings.Get(saveSection, InputInfoKeyKey + actionString, 0).AsInt64(),
                JoyButton = (JoyButton)GameSettings.Get(saveSection, InputInfoJoyButtonKey + actionString, 0).AsInt64(),
                Axis = (JoyAxis)GameSettings.Get(saveSection, InputInfoJoyAxisKey + actionString, 0).AsInt64(),
                AxisSign = GameSettings.Get(saveSection, InputInfoAxisSignKey + actionString, 0).AsSingle()
            };

            SetOverride(actionString, mapping, false);
        }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static InputEvent GetCurrentlyMappedEvent(string action, InputDeviceType type)
    {
        foreach (InputEvent candidate in InputMap.ActionGetEvents(action))
        {
            if (candidate.GetDeviceType() == type)
            {
                return candidate;
            }
        }
        
        return null;
    }
}