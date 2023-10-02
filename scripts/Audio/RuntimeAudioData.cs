namespace Craiel.Essentials;

using System;
using System.Collections.Generic;
using GameData;
using Godot;
using Resource;

[Serializable]
public class RuntimeAudioData : RuntimeGameData
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public RuntimeAudioData()
    {
        this.Clips = new List<string>();
        this.ClipKeys = new List<ResourceKey>();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public AudioPlayBehavior PlayBehavior;

    public bool OnlyOneAtATime;

    public AudioFlags Flags;

    public List<string> Clips;

    public IList<ResourceKey> ClipKeys { get; private set; }

    public override void PostLoad()
    {
        base.PostLoad();

        foreach (string path in this.Clips)
        {
            this.ClipKeys.Add(ResourceKey.Create<AudioStream>(path));
        }
    }
}