﻿namespace Craiel.Essentials.Runtime;

using System;
using System.Collections.Generic;
using EngineCore;
using EngineCore.Modules;
using Event;
using Events;
using Godot;
using I18N;
using IO;
using Logging;
using Resource;
using Threading;
using TweenLite;

public static class EssentialCore
{
    private static readonly IList<IGameModule> ActiveGameModules = new List<IGameModule>();
    
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

    public static ResourceProvider ResourceProvider;
    public static ModuleSaveLoad SaveLoad;
    public static LocalizationSystem Localization;
    public static GameEvents GameEvents;
    public static UIEvents UIEvents;
    public static TweenLiteSystem Tween;
    public static SynchronizationDispatcher Synchronization;
    
    public static void Initialize()
    {
        if (EssentialEngineState.IsInitialized)
        {
            throw new InvalidOperationException("Engine was already initialized!");
        }
        
        ResourceProvider = new ResourceProvider();
        SaveLoad = new ModuleSaveLoad();
        Localization = new LocalizationSystem();
        GameEvents = new GameEvents();
        UIEvents = new UIEvents();
        Tween = new TweenLiteSystem();
        Synchronization = new SynchronizationDispatcher();
    
        // Add Built-in Modules, order matters here
        ActiveGameModules.Add(ResourceProvider);
        ActiveGameModules.Add(SaveLoad);
        ActiveGameModules.Add(Localization);
        ActiveGameModules.Add(GameEvents);
        ActiveGameModules.Add(UIEvents);
        ActiveGameModules.Add(Tween);
        ActiveGameModules.Add(Synchronization);
        
        try
        {
            // Initialize Game Modules
            foreach (IGameModule module in ActiveGameModules)
            {
                Logger.Info("Loading Module: " + module.GetType().Name);
                
                module.Initialize();
            }
            
            // Load Resources
            ResourceProvider.LoadImmediate();
            Localization.Load();

            Logger.Warn("Essential Engine.Initialize() complete");

            EssentialEngineState.IsInitialized = true;
            GameEvents.Send(new EventEngineInitialized());
        }
        catch (Exception e)
        {
            Logger.Error("Error in Initialize of Sub-systems: " + e);
            throw;
        }
    }
    
    public static void Update(double delta)
    {
        for (var i = 0; i < ActiveGameModules.Count; i++)
        {
            ActiveGameModules[i].Update(delta);
        }
    }

    public static void Destroy()
    {
        for (var i = 0; i < ActiveGameModules.Count; i++)
        {
            ActiveGameModules[i].Destroy();
        }
        
        ActiveGameModules.Clear();
    }
}