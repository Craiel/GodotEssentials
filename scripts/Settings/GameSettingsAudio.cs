namespace Craiel.Essentials.Settings;

using System;
using Craiel.Essentials.Audio;
using Godot;

public static class GameSettingsAudio
{
    private const string masterKey = "audio_master";
    private const string musicKey = "audio_music";
    private const string sfxKey = "audio_sfx";
    private const string ambientKey = "audio_ambient";
    private const string uiKey = "audio_ui";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void SetDefaults()
    {
        SetMasterVolume(1.0f, false, false);
        SetMusicVolume(0.7f, false, false);
        SetAmbientVolume(0.80f, false, false);
        SetSFXVolume(0.65f, false, false);
        SetUIVolume(0.55f, false, false);
    }
    
    public static void SetMasterVolume(float newValue, bool save = true, bool apply = true)
    {
        float currentValue = GameSettings.Get(GameSettingsSection.Audio, masterKey, 1f).AsSingle();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
    
        GameSettings.Set(GameSettingsSection.Audio, masterKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static float GetMasterVolume()
    {
        return GameSettings.Get(GameSettingsSection.Audio, masterKey, 1f).AsSingle();
    }
    
    public static void SetMusicVolume(float newValue, bool save = true, bool apply = true)
    {
        float currentValue = GameSettings.Get(GameSettingsSection.Audio, musicKey, 1f).AsSingle();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
    
        GameSettings.Set(GameSettingsSection.Audio, musicKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static float GetMusicVolume()
    {
        return GameSettings.Get(GameSettingsSection.Audio, musicKey, 1f).AsSingle();
    }
    
    public static void SetSFXVolume(float newValue, bool save = true, bool apply = true)
    {
        float currentValue = GameSettings.Get(GameSettingsSection.Audio, sfxKey, 1f).AsSingle();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
    
        GameSettings.Set(GameSettingsSection.Audio, sfxKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static float GetSFXVolume()
    {
        return GameSettings.Get(GameSettingsSection.Audio, sfxKey, 1f).AsSingle();
    }
    
    public static void SetAmbientVolume(float newValue, bool save = true, bool apply = true)
    {
        float currentValue = GameSettings.Get(GameSettingsSection.Audio, ambientKey, 1f).AsSingle();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
    
        GameSettings.Set(GameSettingsSection.Audio, ambientKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static float GetAmbientVolume()
    {
        return GameSettings.Get(GameSettingsSection.Audio, ambientKey, 1f).AsSingle();
    }

    public static void SetUIVolume(float newValue, bool save = true, bool apply = true)
    {
        float currentValue = GameSettings.Get(GameSettingsSection.Audio, uiKey, 1f).AsSingle();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
    
        GameSettings.Set(GameSettingsSection.Audio, uiKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static float GetUIVolume()
    {
        return GameSettings.Get(GameSettingsSection.Audio, uiKey, 1f).AsSingle();
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    public static void Apply()
    {
        AudioController.SetVolume(AudioBus.Master, GetMasterVolume());
        AudioController.SetVolume(AudioBus.Music, GetMusicVolume());
        AudioController.SetVolume(AudioBus.SFX, GetSFXVolume());
        AudioController.SetVolume(AudioBus.Ambient, GetAmbientVolume());
        AudioController.SetVolume(AudioBus.UI, GetUIVolume());
    }
}