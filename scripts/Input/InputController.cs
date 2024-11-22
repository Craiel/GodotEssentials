namespace Craiel.Essentials.Input;

using System;
using System.Collections.Generic;
using Godot;
using Utils;

public static class InputController
{
    private static readonly IList<IInputReceiver> activeReceivers = new List<IInputReceiver>();
    private static readonly IDictionary<string, IList<InputMappingInfo>> mappingCache = new Dictionary<string, IList<InputMappingInfo>>();
    private static readonly IDictionary<string, IList<InputMappingInfo>> defaultMappings = new Dictionary<string, IList<InputMappingInfo>>();
    
    internal static InputLockState InputLock;

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

    public static void SaveDefaultMappingsFromProjectSettings<T>()
        where T: Enum
    {
        defaultMappings.Clear();
        SaveMappings<T>(defaultMappings);
    }

    public static void RegisterMapping(string action, InputMappingInfo info)
    {
        if (!defaultMappings.TryGetValue(action, out IList<InputMappingInfo> infos))
        {
            infos = new List<InputMappingInfo>();
            defaultMappings.Add(action, infos);
        }
        
        infos.Add(info);
    }

    public static void RestoreDefaultMappings(InputDeviceType type)
    {
        foreach (string action in defaultMappings.Keys)
        {
            if (!InputMap.HasAction(action))
            {
                InputMap.AddAction(action);
            }
            
            foreach (var inputEvent in InputMap.ActionGetEvents(action))
            {
                if (inputEvent.GetDeviceType() != type)
                {
                    continue;
                }
                
                InputMap.ActionEraseEvent(action, inputEvent);
            }
            
            foreach (var mapping in defaultMappings[action])
            {
                InputEvent newEvent = mapping.GetEvent();
                if (newEvent.GetDeviceType() != type)
                {
                    continue;
                }
                
                
                InputMap.ActionAddEvent(action, newEvent);
            }
        }
    }
    
    public static void RebuildMappingCache<T>()
        where T: Enum
    {
        mappingCache.Clear();
        SaveMappings<T>(mappingCache);
    }

    private static void SaveMappings<T>(IDictionary<string, IList<InputMappingInfo>> target)
        where T: Enum
    {
        foreach (T inputActionKey in EnumDefInt<T>.Values)
        {
            string mappingString = inputActionKey.ToString();
            if (!target.TryGetValue(mappingString, out IList<InputMappingInfo> infos))
            {
                infos = new List<InputMappingInfo>();
                target.Add(mappingString, infos);
            }
            
            foreach (InputEvent inputEvent in InputMap.ActionGetEvents(mappingString))
            {
                if (inputEvent.GetInfo(out InputMappingInfo info))
                {
                    infos.Add(info);
                }
            }
        }
    }
    
    public static void Process(IInputReceiver receiver)
    {
        switch (InputLock)
        {
            case InputLockState.HardLock:
            {
                return;
            }

            case InputLockState.SoftLock:
            {
                if (!receiver.InputIgnoreLock)
                {
                    return;
                }
                
                break;
            }
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