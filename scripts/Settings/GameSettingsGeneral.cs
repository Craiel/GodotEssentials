namespace Craiel.Essentials.Settings;

using Enums;

public static class GameSettingsGeneral
{
    private const string VibrationKey = "gen_vibration";
    private const string LanguageKey = "gen_language";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void SetDefaults()
    {
        SetVibration(true, false, false);
    }
    
    public static void SetVibration(bool newValue, bool save = true, bool apply = true)
    {
        bool currentValue = GameSettings.Get(GameSettingsSection.General, VibrationKey, true).AsBool();
        if (currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.General, VibrationKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static bool GetVibration()
    {
        return GameSettings.Get(GameSettingsSection.General, VibrationKey, true).AsBool();
    }
    
    public static void SetLanguage(GameLanguage newValue, bool save = true, bool apply = true)
    {
        GameLanguage currentValue = (GameLanguage)GameSettings.Get(GameSettingsSection.General, LanguageKey, (int)GameLanguage.English).AsInt32();
        if (currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.General, VibrationKey, (int)newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static GameLanguage GetLanguage()
    {
        return (GameLanguage)GameSettings.Get(GameSettingsSection.General, LanguageKey, (int)GameLanguage.English).AsInt32();
    }

    public static void Apply()
    {
    }
}