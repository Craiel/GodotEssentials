namespace Craiel.Essentials.Runtime.EngineCore;

using System;
using Event;
using Events;
using I18N;
using Resource;
using Singletons;
using TweenLite;

public abstract partial class EssentialEngineCore<T, TSceneEnum> : GodotSingleton<T>
    where T : EssentialEngineCore<T, TSceneEnum>
    where TSceneEnum: struct, IConvertible
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Initialize()
    {
        if (EssentialEngineState.IsInitialized)
        {
            throw new InvalidOperationException("Engine was already initialized!");
        }
        
        base.Initialize();

        // System components next
        ResourceProvider.InstantiateAndInitialize();
        ResourceStreamProvider.InstantiateAndInitialize();

        // Do localization fairly early so everything can run through it
        LocalizationSystem.InstantiateAndInitialize();

        // LoadFromProto resources all components will need
        this.LoadPermanentResources();

        // First some main systems
        GameEvents.InstantiateAndInitialize();
        // TODO: UIEvents.InstantiateAndInitialize();
        TweenLiteSystem.InstantiateAndInitialize();

        try
        {
            // Initialize Game specific components
            this.InitializeGameComponents();

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

    [UsedImplicitly]
    public virtual void Update()
    {
        if (this.transitioning)
        {
            this.UpdateSceneTransition();
        }
    }

#if UNITY_EDITOR
    public IDictionary<ResourceKey, long> GetHistory()
    {
        return ResourceProvider.Instance.GetHistory();
    }
#endif

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected virtual void LoadPermanentResources()
    {
        // Todo: Add resources we will always need

        ResourceProvider.Instance.LoadImmediate();
    }

    protected abstract void InitializeGameComponents();
}