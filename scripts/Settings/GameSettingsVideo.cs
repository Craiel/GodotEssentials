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
    private const string PositionSettingsKey = "disp_position";

    private static bool lockSetOperations;
    
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
        SetPosition(Vector2.Zero, false, false);
        SetWindowSize(DisplayServer.ScreenGetSize(), false, false);
        SetContentScale(1f, false, false);
        SetVSyncMode(DisplayServer.VSyncMode.Adaptive, false, false);

        DisplayUtils.RefreshWindowSizes(GetScreen());
    }

    public static void SetScreen(int newValue, bool save = true, bool apply = true)
    {
        if (lockSetOperations)
        {
            return;
        }

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
        if (lockSetOperations)
        {
            return;
        }

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
        if (lockSetOperations)
        {
            return;
        }

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
        Vector2I result = GameSettings.Get(GameSettingsSection.Video, WindowSizeSettingsKey, DisplayServer.ScreenGetSize()).AsVector2I();
        return result;
    }

    public static void SetContentScale(double newValue, bool save = true, bool apply = true)
    {
        if (lockSetOperations)
        {
            return;
        }

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
        if (lockSetOperations)
        {
            return;
        }

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
    
    public static void SetPosition(Vector2 newValue, bool save = true, bool apply = true)
    {
        if (lockSetOperations)
        {
            return;
        }
        
        Vector2I currentValue = GameSettings.Get(GameSettingsSection.Video, PositionSettingsKey, Vector2I.Zero).AsVector2I();
        if (currentValue != Vector2I.Zero && currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.Video, PositionSettingsKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static Vector2I GetPosition()
    {
        var result = GameSettings.Get(GameSettingsSection.Video, PositionSettingsKey, Vector2I.Zero).AsVector2I();
        return result;
    }
    
    public static void Apply()
    {
        lockSetOperations = true;

        var mode = GetDisplayMode();

        // Set mode first before adjusting size/position
        DisplayServer.WindowSetMode(mode);

        switch (mode)
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
                DisplayServer.WindowSetPosition(GetPosition());
                break;
            }
        }

        DisplayServer.WindowSetVsyncMode(GetVSyncMode());

        UIEvents.Send(new UIEventScaleFactorChangeRequest((float)GetContentScale()));

        lockSetOperations = false;
    }
}