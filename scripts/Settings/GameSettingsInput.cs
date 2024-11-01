namespace Craiel.Essentials.Settings;

using CanineJRPG.Core;
using Godot;
using Input;
using InputMappingType = Input.InputMappingType;

public static class GameSettingsInput
{
    private const string InputInfoTypeKey = "input_info_type_";
    private const string InputInfoKeyKey = "input_info_key_";
    private const string InputInfoJoyButtonKey = "input_info_joy_button_";
    private const string InputInfoJoyAxisKey = "input_info_joy_axis_";
    private const string InputInfoAxisSignKey = "input_info_axis_sign_";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void ResetToDefault(InputMappingType type)
    {
        // Erase all overrides
        switch (type)
        {
            case InputMappingType.Key:
            {
                GameSettings.Erase(GameSettingsSection.Keyboard);
                break;
            }

            case InputMappingType.JoyButton:
            case InputMappingType.JoyMotion:
            {
                GameSettings.Erase(GameSettingsSection.Gamepad);
                break;
            }
        }
        
        GameSettings.Save();
    }
    
    public static void SetDefaults()
    {
    }
    
    public static void SetOverride(string action, InputMappingInfo info)
    {
        InputEvent currentEvent = GetCurrentlyMappedEvent(action, info.Type);
        if (currentEvent != null)
        {
            InputMap.ActionEraseEvent(action, currentEvent);
        }

        var input = info.GetEvent();
        if (input != null)
        {
            InputMap.ActionAddEvent(action, input);
            InputController.RebuildMappingCache<InputGameEvent>();

            GameSettingsSection saveSection = info.Type == InputMappingType.Key
                ? GameSettingsSection.Keyboard
                : GameSettingsSection.Gamepad;
            
            GameSettings.Set(saveSection, InputInfoTypeKey + action, (ushort)info.Type);
            GameSettings.Set(saveSection, InputInfoKeyKey + action, (long)info.Key);
            GameSettings.Set(saveSection, InputInfoJoyButtonKey + action,(long) info.JoyButton);
            GameSettings.Set(saveSection, InputInfoJoyAxisKey + action, (long)info.Axis);
            GameSettings.Set(saveSection, InputInfoAxisSignKey + action, info.AxisSign);
        }
    }
    
    /*return new InputMappingInfo
              {
                  Type = (InputMappingType)source[InputInfoTypeKey + action].AsUInt16(),
                  Key = (Key)source[InputInfoKeyKey + action].AsInt64(),
                  JoyButton = (JoyButton)source[InputInfoJoyButtonKey + action].AsInt64(),
                  Axis = (JoyAxis)source[InputInfoJoyAxisKey + action].AsInt64(),
                  AxisSign = source[InputInfoAxisSignKey + action].AsSingle(),
              };*/

    public static void Apply()
    {
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static InputEvent GetCurrentlyMappedEvent(string action, InputMappingType type)
    {
        foreach (InputEvent candidate in InputMap.ActionGetEvents(action))
        {
            if (candidate.IsKeyboardEvent() && type == InputMappingType.Key)
            {
                return candidate;
            }

            if (candidate.IsControllerEvent() && type is InputMappingType.JoyButton or InputMappingType.JoyMotion)
            {
                return candidate;
            }
        }
        
        return null;
    }
}