namespace Craiel.Essentials.Input;

using System.Collections.Generic;
using Godot;

public partial class InputBuffer : Node
{
    private static readonly IDictionary<Key, ulong> keyActions = new Dictionary<Key, ulong>();
    private static readonly IDictionary<JoyButton, ulong> joyActions = new Dictionary<JoyButton, ulong>();
    private static readonly IDictionary<string, ulong> joyAxisPress = new Dictionary<string, ulong>();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _Input(InputEvent eventData)
    {
        base._Input(eventData);

        if (InputController.InputLock != InputLockState.None)
        {
            // Won't buffer any input during any lock
            return;
        }

        ulong currentTime = Time.GetTicksMsec();
        if (eventData is InputEventKey eventKey)
        {
            if (!eventKey.Pressed || eventKey.IsEcho())
            {
                return;
            }

            keyActions[eventKey.PhysicalKeycode] = currentTime;
            return;
        }

        if (eventData is InputEventJoypadButton eventJoypadButton)
        {
            if (!eventJoypadButton.Pressed || eventJoypadButton.IsEcho())
            {
                return;
            }
            
            joyActions[eventJoypadButton.ButtonIndex] = currentTime;
            return;
        }

        if (eventData is InputEventJoypadMotion eventJoypadMotion)
        {
            float absAxisValue = Mathf.Abs(eventJoypadMotion.AxisValue);
            if (absAxisValue < InputConstants.DefaultDeadZone)
            {
                return;
            }

            string motionKey = $"{eventJoypadMotion.Axis}_{Mathf.Sign(eventJoypadMotion.AxisValue)}";

            if (absAxisValue >= InputConstants.MotionAsPressThreshold)
            {
                joyAxisPress[motionKey] = currentTime;
            }
            
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

                    if (time - lastActionTime < InputConstants.DefaultBufferWindow)
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

                    if (time - lastActionTime < InputConstants.DefaultBufferWindow)
                    {
                        InvalidateAction(action);
                        return true;
                    }
                    
                    break;
                }

                case InputMappingType.JoyMotion:
                {
                    string motionKey = $"{info.Axis}_{info.AxisSign}";
                    if (!joyAxisPress.TryGetValue(motionKey, out ulong lastActionTime))
                    {
                        break;
                    }

                    if (time - lastActionTime < InputConstants.DefaultBufferWindow)
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
                    // Motion resets all, we can either consume press or motion but not both at the same time
                    string motionKey = $"{info.Axis}_{info.AxisSign}";
                    joyAxisPress[motionKey] = 0;
                    break;
                }
            }
        }
    }
}