namespace Craiel.Essentials.Audio;

using System.Collections.Generic;
using Craiel.Essentials.Resource;
using Godot;

public partial class AudioPlayerNode : Node
{
    private const int SfxPoolSize = 8;
    private const int AmbientPoolSize = 2;
    private const int UiPoolSize = 4;
    private const float MusicCrossfadeDuration = 1.0f;
    private const string PropertyVolumeDb = "volume_db";

    private readonly List<AudioStreamPlayer> sfxPool = new();
    private readonly List<AudioStreamPlayer> ambientPool = new();
    private readonly List<AudioStreamPlayer> uiPool = new();

    private AudioStreamPlayer musicPlayer1;
    private AudioStreamPlayer musicPlayer2;
    private bool musicUsePlayer1 = true;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _EnterTree()
    {
        base._EnterTree();
        AudioController.Player = this;
        this.InitializePools();
    }

    public void Play(AudioBus bus, ResourceKey audioResource)
    {
        if (audioResource == ResourceKey.Invalid)
        {
            EssentialCore.Logger.Warn("Tried to play invalid sound!");
            return;
        }

        var stream = audioResource.LoadManaged<AudioStream>();
        if (stream == null)
        {
            EssentialCore.Logger.Error("Could not load audio stream: " + audioResource);
            return;
        }

        switch (bus)
        {
            case AudioBus.Music:
            {
                this.PlayMusic(stream);
                break;
            }

            case AudioBus.SFX:
            {
                this.PlayFromPool(this.sfxPool, stream);
                break;
            }

            case AudioBus.Ambient:
            {
                this.PlayFromPool(this.ambientPool, stream);
                break;
            }

            case AudioBus.UI:
            {
                this.PlayFromPool(this.uiPool, stream);
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
                this.StopMusic();
                break;
            }

            case AudioBus.SFX:
            {
                this.StopPool(this.sfxPool);
                break;
            }

            case AudioBus.Ambient:
            {
                this.StopPool(this.ambientPool);
                break;
            }

            case AudioBus.UI:
            {
                this.StopPool(this.uiPool);
                break;
            }
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    

    private void InitializePools()
    {
        this.CreatePool(this.sfxPool, AudioBus.SFX, SfxPoolSize);
        this.CreatePool(this.ambientPool, AudioBus.Ambient, AmbientPoolSize);
        this.CreatePool(this.uiPool, AudioBus.UI, UiPoolSize);
        this.CreateMusicPlayers();
    }

    private void CreatePool(List<AudioStreamPlayer> pool, AudioBus bus, int size)
    {
        for (int i = 0; i < size; i++)
        {
            var player = new AudioStreamPlayer();
            player.Bus = bus.ToString();
            this.AddChild(player);
            pool.Add(player);
        }
    }

    private void CreateMusicPlayers()
    {
        this.musicPlayer1 = new();
        this.musicPlayer1.Bus = nameof(AudioBus.Music);
        this.AddChild(this.musicPlayer1);

        this.musicPlayer2 = new();
        this.musicPlayer2.Bus = nameof(AudioBus.Music);
        this.AddChild(this.musicPlayer2);
    }

    private void PlayFromPool(List<AudioStreamPlayer> pool, AudioStream stream)
    {
        if (pool.Count == 0)
        {
            return;
        }

        // Find first non-playing player
        AudioStreamPlayer player = null;
        foreach (var p in pool)
        {
            if (!p.Playing)
            {
                player = p;
                break;
            }
        }

        // If all playing, reuse first one
        player ??= pool[0];

        player.Stop();
        player.Stream = stream;
        player.Play();
    }

    private void StopPool(List<AudioStreamPlayer> pool)
    {
        foreach (var player in pool)
        {
            player.Stop();
        }
    }

    private void PlayMusic(AudioStream stream)
    {
        var currentPlayer = this.musicUsePlayer1 ? this.musicPlayer1 : this.musicPlayer2;
        var nextPlayer = this.musicUsePlayer1 ? this.musicPlayer2 : this.musicPlayer1;

        nextPlayer.Stream = stream;
        nextPlayer.VolumeDb = currentPlayer.VolumeDb;
        nextPlayer.Play();

        var tween = this.CreateTween();
        tween.SetTrans(Tween.TransitionType.Linear);
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetParallel(true);
        tween.TweenProperty(currentPlayer, PropertyVolumeDb, -80f, MusicCrossfadeDuration);
        tween.TweenProperty(nextPlayer, PropertyVolumeDb, 0f, MusicCrossfadeDuration);
        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(() =>
        {
            currentPlayer.Stop();
            this.musicUsePlayer1 = !this.musicUsePlayer1;
        }));
    }

    private void StopMusic()
    {
        var tween = this.CreateTween();
        tween.SetTrans(Tween.TransitionType.Linear);
        tween.SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(
            this.musicUsePlayer1 ? this.musicPlayer1 : this.musicPlayer2,
            PropertyVolumeDb,
            -80f,
            MusicCrossfadeDuration);
        tween.TweenCallback(Callable.From(() =>
        {
            this.musicPlayer1.Stop();
            this.musicPlayer2.Stop();
        }));
    }
}
