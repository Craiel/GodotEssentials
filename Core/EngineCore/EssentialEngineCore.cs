namespace Craiel.Essentials.Runtime.EngineCore;

using System;
using System.Collections.Generic;
using System.Linq;
using Collections;
using Event;
using Events;
using I18N;
using Modules;
using Resource;
using Singletons;
using Threading;
using TweenLite;

public abstract partial class EssentialEngineCore<T, TSceneEnum> : GodotSingleton<T>
    where T : EssentialEngineCore<T, TSceneEnum>
    where TSceneEnum: struct, IConvertible
{
    private IGameModule[] gameModules;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public ModuleSaveLoad SaveLoad { get; private set; }
    
    public override void Initialize()
    {
        if (EssentialEngineState.IsInitialized)
        {
            throw new InvalidOperationException("Engine was already initialized!");
        }
        
        base.Initialize();

        // System components next
        ResourceProvider.InstantiateAndInitialize();

        // Do localization fairly early so everything can run through it
        LocalizationSystem.InstantiateAndInitialize();

        // LoadFromProto resources all components will need
        this.LoadPermanentResources();

        // First some main systems
        GameEvents.InstantiateAndInitialize();
        // TODO: UIEvents.InstantiateAndInitialize();
        TweenLiteSystem.InstantiateAndInitialize();
        UnitySynchronizationDispatcher.InstantiateAndInitialize();

        try
        {
            // Initialize custom Engine Components
            this.InitializeEngineComponents();
            
            // Initialize Game Modules
            foreach (IGameModule module in this.gameModules)
            {
                module.Initialize();
            }

            EssentialsCore.Logger.Warn("Essential Engine.Initialize() complete");

            EssentialEngineState.IsInitialized = true;
            GameEvents.Send(new EventEngineInitialized());
        }
        catch (Exception e)
        {
            EssentialsCore.Logger.Error("Error in Initialize of Sub-systems: " + e);
            throw;
        }
    }

    public virtual void Update(double delta)
    {
        TweenLiteSystem.Instance.Update(delta);
        UnitySynchronizationDispatcher.Instance.Update(delta);
        
        if (this.transitioning)
        {
            this.UpdateSceneTransition();
        }
        
        for (var i = 0; i < this.gameModules.Length; i++)
        {
            this.gameModules[i].Update(delta);
        }
    }

    public override void DestroySingleton()
    {
        for (var i = 0; i < this.gameModules.Length; i++)
        {
            this.gameModules[i].Destroy();
        }
        
        base.DestroySingleton();
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected virtual void LoadPermanentResources()
    {
        // Todo: Add resources we will always need

        ResourceProvider.Instance.LoadImmediate();
    }

    protected abstract void InitializeEngineComponents();
    
    protected void SetGameModules(bool includeDefaultModules = true, params IGameModule[] newModules)
    {
        using (TempList<IGameModule> moduleList = new TempList<IGameModule>())
        {
            if (includeDefaultModules)
            {
                moduleList.Add(this.SaveLoad);
            }

            if (newModules == null || newModules.Length == 0)
            {
                this.SetGameModules(moduleList.List);
                return;
            }

            foreach (IGameModule module in newModules)
            {
                if (moduleList.Contains(module))
                {
                    continue;
                }

                moduleList.Add(module);
            }

            this.SetGameModules(moduleList.List);
        }
    }
    
    private void SetGameModules(IList<IGameModule> moduleList)
    {
        if (moduleList == null)
        {
            this.gameModules = Array.Empty<IGameModule>();
            return;
        }
        
        this.gameModules = moduleList.ToArray();
    }
}