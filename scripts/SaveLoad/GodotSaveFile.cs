namespace Craiel.Essentials.SaveLoad;

using System;
using System.Collections.Generic;
using Craiel.Essentials;
using Godot;
using Godot.Collections;

public class GodotSaveFile
{
    private const string VersionKey = "ver";
    private const string FileNameBase = "user://{0}.dat";
    
    private readonly IDictionary<ushort, Action<Dictionary>> upgrades = new System.Collections.Generic.Dictionary<ushort, Action<Dictionary>>();
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public GodotSaveFile(string name, ushort currentVersion)
    {
        FilePath = string.Format(FileNameBase, name);
        CurrentVersion = currentVersion;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public string FilePath { get; }
    public ushort CurrentVersion { get; }
    
    public bool Load(out Dictionary data)
    {
        try
        {
            using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                EssentialCore.Logger.Warn($"Save file not found: {FilePath}");
                data = null;
                return false;
            }

            string json = file.GetPascalString();
            data = (Dictionary)Json.ParseString(json);

            if (data.Count == 0 || !data.ContainsKey(VersionKey))
            {
                EssentialCore.Logger.Warn($"Save file invalid or missing version: {FilePath}");
                return false;
            }

            ApplyUpgrades(data);
            return true;
        }
        catch (Exception e)
        {
            EssentialCore.Logger.Error($"Failed to load save file: {FilePath}", e);
            data = null;
            return false;
        }
    }

    public void Save(Dictionary data)
    {
        try
        {
            // Set the version
            data[VersionKey] = CurrentVersion;
            
            using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                throw new InvalidOperationException($"Failed to open file for writing: {FilePath}");
            }

            string json = Json.Stringify(data, indent: "  ");
            file.StorePascalString(json);
            file.Close();
        }
        catch (Exception e)
        {
            EssentialCore.Logger.Error($"Failed to save file: {FilePath}", e);
            throw;
        }
    }
    
    public void AddUpgrade(ushort version, Action<Dictionary> upgrade)
    {
        this.upgrades.Add(version, upgrade);
    }

    public void Delete()
    {
        if (FileAccess.FileExists(FilePath))
        {
            DirAccess.RemoveAbsolute(FilePath);
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void ApplyUpgrades(Dictionary data)
    {
        ushort dataVersion = data[VersionKey].AsUInt16();
        while (dataVersion < CurrentVersion)
        {
            if (this.upgrades.TryGetValue(dataVersion, out Action<Dictionary> upgradeAction))
            {
                try
                {
                    upgradeAction(data);
                    data[VersionKey] = dataVersion + 1;
                }
                catch (Exception e)
                {
                    EssentialCore.Logger.Error($"Failed to apply upgrade to version {dataVersion}", e);
                    throw;
                }
            }
            
            dataVersion++;
        }
    }
}
