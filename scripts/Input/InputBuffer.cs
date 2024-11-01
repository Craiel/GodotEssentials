namespace Craiel.Essentials.Input;

using System.Collections.Generic;
using Godot;

public partial class InputBuffer : Node
{
    private const double BufferWindow = 150;
    private const float JoyDeadZone = 0.2f;

    private static readonly IDictionary<Key, ulong> keyActions = new Dictionary<Key, ulong>();
    private static readonly IDictionary<JoyButton, ulong> joyActions = new Dictionary<JoyButton, ulong>();
    private static readonly IDictionary<string, ulong> joyMotions = new Dictionary<string, ulong>();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _Input(InputEvent eventData)
    {
        base._Input(eventData);

        if (InputController.InputLocked)
        {
            // Won't buffer any input during lock
            return;
        }

        if (eventData is InputEventKey eventKey)
        {
            if (!eventKey.Pressed || eventKey.IsEcho())
            {
                return;
            }

            keyActions[eventKey.PhysicalKeycode] = Time.GetTicksMsec();
            return;
        }

        if (eventData is InputEventJoypadButton eventJoypadButton)
        {
            if (!eventJoypadButton.Pressed || eventJoypadButton.IsEcho())
            {
                return;
            }
            
            joyActions[eventJoypadButton.ButtonIndex] = Time.GetTicksMsec();
            return;
        }

        if (eventData is InputEventJoypadMotion eventJoypadMotion)
        {
            if (Mathf.Abs(eventJoypadMotion.AxisValue) < JoyDeadZone)
            {
                return;
            }

            joyMotions[$"{eventJoypadMotion.Axis}_{Mathf.Sign(eventJoypadMotion.AxisValue)}"] = Time.GetTicksMsec();
            return;
        }
    }

    public static bool IsActionPressed(string action)
    {
        IList<InputMappingInfo> infos = InputController.GetInfo(action);
        if (infos == null)
        {
            return false;
        }

        ulong time = Time.GetTicksMsec();
        foreach (InputMappingInfo info in infos)
        {
            switch (info.Type)
            {
                case InputMappingType.Key:
                {
                    if (!keyActions.TryGetValue(info.Key, out ulong lastActionTime))
                    {
                        break;
                    }

                    if (time - lastActionTime < BufferWindow)
                    {
                        InvalidateAction(action);
                        return true;
                    }
                    
                    break;
                }

                case InputMappingType.JoyButton:
                {
                    if (!joyActions.TryGetValue(info.JoyButton, out ulong lastActionTime))
                    {
                        break;
                    }

                    if (time - lastActionTime < BufferWindow)
                    {
                        InvalidateAction(action);
                        return true;
                    }
                    
                    break;
                }

                case InputMappingType.JoyMotion:
                {
                    string motionKey = $"{info.Axis}_{info.AxisSign}";
                    if (!joyMotions.TryGetValue(motionKey, out ulong lastActionTime))
                    {
                        break;
                    }

                    if (time - lastActionTime < BufferWindow)
                    {
                        InvalidateAction(action);
                        return true;
                    }
                    
                    break;
                }
            }
        }

        return false;
    }

    private static void InvalidateAction(string action)
    {
        IList<InputMappingInfo> infos = InputController.GetInfo(action);
        if (infos == null)
        {
            return;
        }
        
        foreach (InputMappingInfo info in infos)
        {
            switch (info.Type)
            {
                case InputMappingType.Key:
                {
                    keyActions[info.Key] = 0;
                    break;
                }

                case InputMappingType.JoyButton:
                {
                    joyActions[info.JoyButton] = 0;
                    break;
                }

                case InputMappingType.JoyMotion:
                {
                    string motionKey = $"{info.Axis}_{info.AxisSign}";
                    joyMotions[motionKey] = 0;
                    break;
                }
            }
        }
    }
}