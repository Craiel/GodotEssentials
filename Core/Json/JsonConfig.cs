namespace Craiel.Essentials.Runtime.Json;

using System;
using System.Text.Json;
using Contracts;
using IO;
using Debug = System.Diagnostics.Debug;

public class JsonConfig<T> : IJsonConfig<T>
    where T : class
{
    private ManagedFile configFile;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public T Current { get; set; }

    public virtual bool Load(ManagedFile file)
    {
        this.configFile = file;
        return this.LoadConfig(this.configFile);
    }

    public virtual bool Save(ManagedFile file = null)
    {
        ManagedFile targetFile = file ?? this.configFile;
        Debug.Assert(targetFile != null);

        try
        {
            string contents = JsonSerializer.Serialize(this.Current);
            targetFile.WriteAsString(contents);
            return true;
        }
        catch (Exception e)
        {
            EssentialCore.Logger.Error($"Could not save config to {file}", e);
            return false;
        }
    }

    public virtual void Reset()
    {
        this.Current = this.GetDefault();
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected virtual T GetDefault()
    {
        return Activator.CreateInstance<T>();
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private bool LoadConfig(ManagedFile file)
    {
        if (file.Exists)
        {
            string contents = file.ReadAsString();
            this.Current = JsonSerializer.Deserialize<T>(contents);
        }
        else
        {
            EssentialCore.Logger.Warn($"Config {file} does not exist, skipping");
        }

        if (this.Current == null)
        {
            EssentialCore.Logger.Error("Config is invalid, resetting to default");
            this.Current = this.GetDefault();

            string contents = JsonSerializer.Serialize(this.Current);
            file.WriteAsString(contents);
            return false;
        }

        return true;
    }
}