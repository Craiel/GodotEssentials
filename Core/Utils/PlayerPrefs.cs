namespace Craiel.Essentials.Runtime.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Extensions;
using FileAccess = Godot.FileAccess;

public static class PlayerPrefs
{
    private const string PrefsFile = "user://playerprefs.data";
    
    private static readonly List<string> keys = new();
    private static readonly List<string> values = new();

    [Serializable]
    struct JsonData
    {
        public string[] Keys;
        public string[] Values;
    }

    static PlayerPrefs()
    {
    }

    static void Load()
    {
        keys.Clear();
        values.Clear();
        
        if (!FileAccess.FileExists(PrefsFile))
        {
            return;
        }
        
        var prefsFile = FileAccess.Open(PrefsFile, FileAccess.ModeFlags.Read);
        string serialized = prefsFile.GetAsText();
        if (string.IsNullOrEmpty(serialized))
        {
            return;
        }

        var data = JsonSerializer.Deserialize<JsonData>(serialized);
        if (data.Keys.IsNullOrEmpty() || data.Values.IsNullOrEmpty())
        {
            return;
        }

        if (data.Keys.Length != data.Values.Length)
        {
            throw new InvalidDataException("Inconsistent Data");
        }

        for (var i = 0; i < data.Keys.Length; i++)
        {
            keys.Add(data.Keys[i]);
            values.Add(data.Values[i]);
        }
    }

    public static void Save()
    {
        var data = new JsonData
        {
            Keys = keys.ToArray(),
            Values = values.ToArray()
        };

        string serialized = JsonSerializer.Serialize(data);
        if (string.IsNullOrEmpty(serialized))
        {
            throw new InvalidOperationException("Data Failed to Serialize");
        }
        
        var prefsFile = FileAccess.Open(PrefsFile, FileAccess.ModeFlags.Write);
        prefsFile.StoreString(serialized);
    }

    public static string GetString(string key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            return values[index];
        }

        return null;
    }

    public static void SetString(string key, string value)
    {
        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            values[index] = value;
            return;
        }
        
        keys.Add(key);
        values.Add(value);
    }

    public static void DeleteKey(string key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
        }
    }
}