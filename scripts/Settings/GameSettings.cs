namespace Craiel.Essentials.Settings;

using Godot;

public static class GameSettings
{
    private const string ConfigPath = "user://settings.cfg";
    private const string SystemSection = "system";
    private const string FirstStartKey = "first_start";

    private static readonly ConfigFile Config;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    static GameSettings()
    {
        Config = new ConfigFile();
        if (FileAccess.FileExists(ConfigPath))
        {
            Config.Load(ConfigPath);
        }
        
        bool isFirstStart = Get(GameSettingsSection.SaveLoad, FirstStartKey, true).AsBool();
        if (!isFirstStart)
        {
            Set(GameSettingsSection.SaveLoad, FirstStartKey, false);
            GameSettingsGeneral.SetDefaults();
            GameSettingsAudio.SetDefaults();
            GameSettingsVideo.SetDefaults();
            GameSettingsInput.SetDefaults();
        }
        
        GameSettingsGeneral.Apply();
        GameSettingsAudio.Apply();
        GameSettingsVideo.Apply();
        GameSettingsInput.Apply();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void Save()
    {
        Config.Save(ConfigPath);
    }

    public static void Set(GameSettingsSection section, string key, Variant value)
    {
        Config.SetValue(section.ToString(), key, value);
    }

    public static void Erase(GameSettingsSection section)
    {
        Config.EraseSection(section.ToString());
    }

    public static Variant Get(GameSettingsSection section, string key, Variant defaultValue = default)
    {
        return Config.GetValue(section.ToString(), key, defaultValue);
    }
}