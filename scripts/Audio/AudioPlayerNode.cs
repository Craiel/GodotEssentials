namespace Craiel.Essentials.Audio;

using System.IO;
using Craiel.Essentials.Resource;
using Godot;

public partial class AudioPlayerNode : Node
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] private AudioStreamPlayer MusicStatic;
    [Export] private AudioStreamPlayer SFXStatic;
    [Export] private AudioStreamPlayer AmbienceStatic;
    [Export] private AudioStreamPlayer UIStatic;

    public override void _EnterTree()
    {
        base._EnterTree();

        AudioController.Player = this;
    }

    public void Play(AudioBus bus, ResourceKey audioResource)
    {
        switch (bus)
        {
            case AudioBus.Music:
            {
                Play(this.MusicStatic, audioResource);
                break;
            }

            case AudioBus.SFX:
            {
                Play(this.SFXStatic, audioResource);
                break;
            }

            case AudioBus.Ambient:
            {
                Play(this.AmbienceStatic, audioResource);
                break;
            }

            case AudioBus.UI:
            {
                Play(this.UIStatic, audioResource);
                break;
            }
        }
    }
    
    public void Stop(AudioBus bus)
    {
        switch (bus)
        {
            case AudioBus.Music:
            {
                Stop(this.MusicStatic);
                break;
            }

            case AudioBus.SFX:
            {
                this.Stop(this.SFXStatic);
                break;
            }

            case AudioBus.Ambient:
            {
                this.Stop(this.AmbienceStatic);
                break;
            }

            case AudioBus.UI:
            {
                this.Stop(this.UIStatic);
                break;
            }
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void Play(AudioStreamPlayer player, ResourceKey key)
    {
        if (key == ResourceKey.Invalid)
        {
            EssentialCore.Logger.Warn("Tried to play invalid sound!");
            return;
        }
        
        var stream = key.LoadManaged<AudioStream>();
        if (stream == null)
        {
            throw new InvalidDataException("Could not load audio stream: " + key);
        }

        player.Stop();
        player.Stream = stream;
        player.Play();
    }

    private void Stop(AudioStreamPlayer player)
    {
        player.Stop();
    }
}