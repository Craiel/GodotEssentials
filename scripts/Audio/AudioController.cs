namespace Craiel.Essentials.Audio;

using System.Collections.Generic;
using Craiel.Essentials.Resource;
using Craiel.Essentials.Utils;
using Godot;

public static class AudioController
{
    private static readonly IDictionary<AudioBus, int> busMapping = new Dictionary<AudioBus, int>();
    
    static AudioController()
    {
        busMapping.Clear();
        foreach (AudioBus bus in EnumDefInt<AudioBus>.Values)
        {
            int index = AudioServer.GetBusIndex(bus.ToString());
            busMapping.Add(bus, index);
        }
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static AudioPlayerNode Player;
    
    public static void SetVolume(AudioBus bus, float volume)
    {
        AudioServer.SetBusVolumeDb(busMapping[bus], Mathf.LinearToDb(volume));
    }

    public static void Play(AudioBus bus, ResourceKey resourceKey)
    {
        Player?.Play(bus, resourceKey);
    }
    
    public static void Stop(AudioBus bus)
    {
        Player?.Stop(bus);
    }
}