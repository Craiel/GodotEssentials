namespace Craiel.Essentials.Settings;

using System;
using Craiel.Essentials.Utils;
using Event;
using Events.UI;
using Godot;

public static class GameSettingsVideo
{
    private const string ScreenSettingsKey = "disp_screen";
    private const string DisplayModeSettingsKey = "disp_mode";
    private const string WindowSizeSettingsKey = "disp_win_size";
    private const string ContentScaleSettingsKey = "disp_content_scale";
    private const string VSyncSettingsKey = "disp_vsync";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void Refresh()
    {
        DisplayUtils.RefreshWindowSizes(GetScreen());
    }
    
    public static void SetDefaults()
    {
        SetScreen(DisplayServer.GetPrimaryScreen(), false, false);
        SetDisplayMode(DisplayServer.WindowMode.Fullscreen, false, false);
        SetWindowSize(DisplayServer.ScreenGetSize(), false, false);
        SetContentScale(1f, false, false);
        SetVSyncMode(DisplayServer.VSyncMode.Adaptive, false, false);

        DisplayUtils.RefreshWindowSizes(GetScreen());
    }

    public static void SetScreen(int newValue, bool save = true, bool apply = true)
    {
        int currentValue = GameSettings.Get(GameSettingsSection.Video, ScreenSettingsKey, -1).AsInt32();
        if (currentValue >= 0 && currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, ScreenSettingsKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
        
        DisplayUtils.RefreshWindowSizes(GetScreen());
    }

    public static int GetScreen()
    {
        return GameSettings.Get(GameSettingsSection.Video, ScreenSettingsKey, DisplayServer.ScreenPrimary).AsInt32();
    }
    
    public static void SetDisplayMode(DisplayServer.WindowMode mode, bool save = true, bool apply = true)
    {
        short currentValue = GameSettings.Get(GameSettingsSection.Video, DisplayModeSettingsKey, (short)-1).AsInt16();
        short newValue = (short)mode;
        if (currentValue >= 0 && currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, DisplayModeSettingsKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static DisplayServer.WindowMode GetDisplayMode()
    {
        short currentValue = GameSettings.Get(GameSettingsSection.Video, DisplayModeSettingsKey, (short)-1).AsInt16();
        return (DisplayServer.WindowMode)currentValue;
    }

    public static void SetWindowSize(Vector2I newValue, bool save = true, bool apply = true)
    {
        Vector2I currentValue = GameSettings.Get(GameSettingsSection.Video, WindowSizeSettingsKey, Vector2I.Zero).AsVector2I();
        if (currentValue != Vector2I.Zero && currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, WindowSizeSettingsKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static Vector2I GetWindowSize()
    {
        return GameSettings.Get(GameSettingsSection.Video, WindowSizeSettingsKey, DisplayServer.ScreenGetSize()).AsVector2I();
    }

    public static void SetContentScale(double newValue, bool save = true, bool apply = true)
    {
        double currentValue = GameSettings.Get(GameSettingsSection.Video, ContentScaleSettingsKey, 1f).AsDouble();
        if (Math.Abs(currentValue - newValue) < Mathf.Epsilon)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, ContentScaleSettingsKey, newValue);
        
        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static double GetContentScale()
    {
        return GameSettings.Get(GameSettingsSection.Video, ContentScaleSettingsKey, 1f).AsDouble();
    }
    
    public static void SetVSyncMode(DisplayServer.VSyncMode mode, bool save = true, bool apply = true)
    {
        short currentValue = GameSettings.Get(GameSettingsSection.Video, VSyncSettingsKey, (short)-1).AsInt16();
        short newValue = (short)mode;
        if (currentValue >= 0 && currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, VSyncSettingsKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static DisplayServer.VSyncMode GetVSyncMode()
    {
        short currentValue = GameSettings.Get(GameSettingsSection.Video, VSyncSettingsKey, (short)DisplayServer.VSyncMode.Adaptive).AsInt16();
        return (DisplayServer.VSyncMode)currentValue;
    }
    
    public static void Apply()
    {
        switch (GetDisplayMode())
        {
            case DisplayServer.WindowMode.ExclusiveFullscreen:
            case DisplayServer.WindowMode.Fullscreen:
            {
                DisplayServer.WindowSetCurrentScreen(GetScreen());
                break;
            }

            case DisplayServer.WindowMode.Maximized:
            {
                DisplayServer.WindowSetSize(DisplayServer.ScreenGetSize());
                break;
            }
            
            case DisplayServer.WindowMode.Windowed:
            {
                DisplayServer.WindowSetSize(GetWindowSize());
                break;
            }
        }
        
        DisplayServer.WindowSetMode(GetDisplayMode());
        DisplayServer.WindowSetVsyncMode(GetVSyncMode());
        
        UIEvents.Send(new UIEventScaleFactorChangeRequest((float)GetContentScale()));
    }
}