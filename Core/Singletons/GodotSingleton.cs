namespace Craiel.Essentials.Runtime.Singletons;

using Contracts;
using Utils;

public abstract class GodotSingleton<T> : IGodotSingleton
    where T : class, IGodotSingleton
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static bool IsInstanceActive
    {
        get
        {
            return Instance != null;
        }
    }

    public static T Instance { get; private set; }

    public bool IsInitialized { get; protected set; }

    public static void Instantiate()
    {
        // Prevent accidential duplicate initialization
        if (Instance != null)
        {
            EssentialsCore.Logger.Warn($"Prevented duplicate Singleton Instance: {TypeCache<T>.Value}");
            return;
        }

        Instance = new T();

#if DEBUG
        EssentialsCore.Logger.Info($"Singleton.Instantiate: {TypeCache<T>.Value}");
#endif
    }

    public static void InstantiateAndInitialize()
    {
        if (Instance != null && Instance.IsInitialized)
        {
            // Instance is already there and initialized, skip
            return;
        }

        Instantiate();
        Instance.Initialize();
    }

    public virtual void Initialize()
    {
#if DEBUG
        EssentialsCore.Logger.Info($"Singleton.Initialize: {this.GetType().Name}");
#endif

        this.IsInitialized = true;
    }

    public static void DestroyInstance()
    {
        if (IsInstanceActive)
        {
            Instance.DestroySingleton();
        }
    }

    public virtual void DestroySingleton()
    {
#if DEBUG
        EssentialsCore.Logger.Info($"Singleton.Destroy: {this.GetType().Name}");
#endif

        Instance = null;
    }
}
