namespace Craiel.Essentials.Input;

using System;
using System.Collections.Generic;
using Godot;
using Utils;

public static class InputController
{
    private static readonly IList<IInputReceiver> activeReceivers = new List<IInputReceiver>();
    private static readonly IDictionary<string, IList<InputMappingInfo>> mappingCache = new Dictionary<string, IList<InputMappingInfo>>();
    
    internal static bool InputLocked;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static IList<InputMappingInfo> GetInfo(string mapping)
    {
        if (mappingCache.TryGetValue(mapping, out IList<InputMappingInfo> infos))
        {
            return infos;
        }

        return null;
    }
    
    public static void RebuildMappingCache<T>()
        where T: Enum
    {
        mappingCache.Clear();
        foreach (T inputActionKey in EnumDefInt<T>.Values)
        {
            string mappingString = inputActionKey.ToString();
            if (!mappingCache.TryGetValue(mappingString, out IList<InputMappingInfo> infos))
            {
                infos = new List<InputMappingInfo>();
                mappingCache.Add(mappingString, infos);
            }
            
            foreach (InputEvent inputEvent in InputMap.ActionGetEvents(mappingString))
            {
                if (inputEvent is InputEventKey key)
                {
                    var info = new InputMappingInfo
                    {
                        Type = InputMappingType.Key,
                        Key = key.PhysicalKeycode
                    };
                    
                    infos.Add(info);
                    continue;
                }

                if (inputEvent is InputEventJoypadButton joyButton)
                {
                    var info = new InputMappingInfo
                    {
                        Type = InputMappingType.JoyButton,
                        JoyButton = joyButton.ButtonIndex
                    };
                    
                    infos.Add(info);
                    continue;
                }

                if (inputEvent is InputEventJoypadMotion joyMotion)
                {
                    var info = new InputMappingInfo
                    {
                        Type = InputMappingType.JoyMotion,
                        Axis = joyMotion.Axis,
                        AxisSign = Mathf.Sign(joyMotion.AxisValue)
                    };
                    
                    infos.Add(info);
                }
            }
        }
    }
    
    public static void Process(IInputReceiver receiver)
    {
        if (InputLocked && !receiver.InputIgnoreLock)
        {
            return;
        }
        
        receiver.ProcessInput();
    }

    public static void Register(IInputReceiver receiver)
    {
        activeReceivers.Add(receiver);
    }

    public static void Unregister(IInputReceiver receiver)
    {
        activeReceivers.Remove(receiver);
    }
}