namespace Craiel.Essentials.Runtime;

using System;
using Godot;
using IO;
using Logging;

public static class EssentialsCore
{
    public const int LogFileArchiveSize = 10485760;
    public const int LogFileArchiveCount = 10;
    
    public const string LocalizationIgnoreString = "XX_";
    
    public static RandomNumberGenerator Random = new();
    
    public static readonly string ProjectName = ProjectSettings.GetSetting("application/config/name").AsString();
    
    public static readonly ManagedDirectory AppDataPath = new(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
    public static readonly ManagedDirectory PersistentDataPath = AppDataPath.ToDirectory(ProjectName);
    public static readonly ManagedDirectory DefaultSavePath = PersistentDataPath.ToDirectory("Save");
    public const string DefaultSavePrefix = "esv_";

    public static GodotLogRelay Logger = new();

    public static float GameTime => Time.GetTicksMsec() / 1000f;
    
    public static float StartTime = GameTime;
}