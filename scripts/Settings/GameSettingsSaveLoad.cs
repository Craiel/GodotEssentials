namespace Craiel.Essentials.Settings;

public static class GameSettingsSaveLoad
{
    private const string LastSaveSlotKey = "last_save_slot";
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void SetDefaults()
    {
    }

    public static void Apply()
    {
    }
    
    public static void SetLastSaveSlot(int newValue, bool save = true, bool apply = true)
    {
        int currentValue = GameSettings.Get(GameSettingsSection.General, LastSaveSlotKey, -1).AsInt32();
        if (currentValue == newValue)
        {
            return;
        }
        
        GameSettings.Set(GameSettingsSection.General, LastSaveSlotKey, newValue);

        if (save)
        {
            GameSettings.Save();
        }

        if (apply)
        {
            Apply();
        }
    }

    public static int GetLastSaveSlot()
    {
        return GameSettings.Get(GameSettingsSection.General, LastSaveSlotKey, -1).AsInt32();
    }
}