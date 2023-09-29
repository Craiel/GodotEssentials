namespace Craiel.Essentials.Runtime;

using Godot;
using IO;
using Logging;

public static class EssentialsCore
{
    public static readonly string ProjectName = ProjectSettings.GetSetting("application/config/name").AsString();
    
    public static readonly ManagedDirectory AppDataPath = new(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
    public static readonly ManagedDirectory PersistentDataPath = AppDataPath.ToDirectory(ProjectName);
    public static readonly ManagedDirectory DefaultSavePath = PersistentDataPath.ToDirectory("Save");
    public const string DefaultSavePrefix = "esv_";

    public static GodotLogRelay Logger = new();

    private static ManagedDirectory dataPath;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    static EssentialsCore()
    {
        ;
        LocalizationSaveInterval = 60f;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static float LocalizationSaveInterval { get; set; }
}
