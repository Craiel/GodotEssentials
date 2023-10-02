namespace Craiel.Essentials.Resource;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EngineCore;
using Enums;
using Godot;
using Utils;

public delegate void OnResourceLoadingDelegate(ResourceLoadInfo info);
public delegate void OnResourceLoadedDelegate(ResourceLoadInfo info, long loadTime);

public class ResourceProvider : IGameModule
{
    private const int DefaultRequestPoolSize = 30;

    private const int MaxConsecutiveSyncCallsInAsync = 20;

    private readonly ResourceMap<ResourceLoadRequest> resourceMap;

    private readonly IDictionary<ResourceKey, int> referenceCount;

    private readonly Queue<ResourceLoadInfo> currentPendingLoads;

    private readonly ResourceRequestPool<ResourceLoadRequest> requestPool;

    private readonly IDictionary<ResourceKey, long> history;

    private readonly IDictionary<Type, ResourceKey> fallbackResources;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ResourceProvider()
    {
        this.resourceMap = new ResourceMap<ResourceLoadRequest>();
        this.referenceCount = new Dictionary<ResourceKey, int>();

        this.currentPendingLoads = new Queue<ResourceLoadInfo>();

        this.requestPool = new ResourceRequestPool<ResourceLoadRequest>(DefaultRequestPoolSize);

        this.history = new Dictionary<ResourceKey, long>();

        this.fallbackResources = new Dictionary<Type, ResourceKey>();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public event OnResourceLoadingDelegate ResourceLoading;
    public event OnResourceLoadedDelegate ResourceLoaded;

    public int PendingForLoad
    {
        get
        {
            return this.currentPendingLoads.Count;
        }
    }

    public int ResourcesLoaded { get; private set; }

    public bool EnableHistory { get; set; }

    public ResourceRequestPool<ResourceLoadRequest> RequestPool
    {
        get
        {
            return this.requestPool;
        }
    }
    
    public void Initialize()
    {
    }

    public void Update(double delta)
    {
    }

    public void Destroy()
    {
    }

    public static T SingletonResource<T>()
        where T : Resource
    {
        IList<ResourceKey> resources = EssentialCore.ResourceProvider.AcquireResourcesByType<T>();
        if (resources == null || resources.Count != 1)
        {
            EssentialCore.Logger.Warn($"Expected 1 result for {TypeCache<T>.Value}");
            return null;
        }

        return EssentialCore.ResourceProvider.AcquireResource<T>(resources.First()).Data;
    }

    public static Resource LoadImmediate(ResourceKey key)
    {
        if (key.Type != null && !key.Type.IsSubclassOf(TypeCache<Resource>.Value))
        {
            throw new InvalidOperationException("Resource Key requested with non-resource type: " + key);
        }
        
        string typeHint = key.Type != null ? key.Type.Name : TypeCache<Resource>.Value.Name;
        return ResourceLoader.Load(key.Path, typeHint);
    }

    // Note: Use this only when we can not do an async loading, avoid if possible
    public static T LoadImmediate<T>(ResourceKey key)
        where T : Resource
    {
        return LoadImmediate(key) as T;
    }

    public IList<ResourceKey> AcquireResourcesByType<T>()
    {
        return this.resourceMap.GetKeysByType<T>();
    }

    public IDictionary<ResourceKey, long> GetHistory()
    {
        return this.history;
    }

    public void RegisterLoadedResource(ResourceKey key, Resource resource)
    {
        Debug.Assert(resource != null, "Registering a loaded resource with null data!");

        // Register the resource without queuing
        ResourceLoadRequest request = new ResourceLoadRequest(new ResourceLoadInfo(key, ResourceLoadFlags.None), resource);
        this.resourceMap.RegisterResource(key, request);
    }

    public void RegisterResource(ResourceKey key, ResourceLoadFlags flags = ResourceLoadFlags.Cache)
    {
        if ((flags & ResourceLoadFlags.Cache) != 0)
        {
            // Cache the resource in the map
            this.resourceMap.RegisterResource(key);
        }

        lock (this.currentPendingLoads)
        {
            this.currentPendingLoads.Enqueue(new ResourceLoadInfo(key, flags));
        }
    }

    public void RegisterFallbackResource(ResourceKey key, ResourceLoadFlags flags = ResourceLoadFlags.Cache)
    {
        if (this.fallbackResources.ContainsKey(key.Type))
        {
            EssentialCore.Logger.Warn($"Duplicate fallback resource registered for type {key.Type}");
            return;
        }

        this.RegisterResource(key, flags);
        this.fallbackResources.Add(key.Type, key);
    }

    public void UnregisterResource(ResourceKey key)
    {
        this.resourceMap.UnregisterResource(key);
    }

    public void RegisterLink(ResourceKey source, ResourceKey target)
    {
        this.resourceMap.RegisterLink(source, target);
    }

    public void UnregisterLink(ResourceKey source)
    {
        this.resourceMap.UnregisterLink(source);
    }

    public ResourceReference<T> AcquireOrLoadResource<T>(ResourceKey key, ResourceLoadFlags flags = ResourceLoadFlags.Cache)
        where T : Resource
    {
        ResourceReference<T> result;
        if (!this.TryAcquireOrLoadResource(key, out result, flags))
        {
            EssentialCore.Logger.Error("Could not load resource on-demand");
        }

        return result;
    }

    public bool TryAcquireOrLoadResource<T>(
        ResourceKey key,
        out ResourceReference<T> reference,
        ResourceLoadFlags flags = ResourceLoadFlags.Cache) where T : Resource
    {
        reference = null;
        ResourceLoadRequest request = this.resourceMap.GetData(key);
        Resource data = request != null ? request.GetAsset() : null;
        if (data == null)
        {
            this.DoLoadImmediate(new ResourceLoadInfo(key, flags));
            request = this.resourceMap.GetData(key);
            data = request != null ? request.GetAsset() : null;
            if (data == null)
            {
                return false;
            }
        }

        reference = this.BuildReference<T>(key, data);
        return true;
    }

    public ResourceReference<T> AcquireResource<T>(ResourceKey key)
        where T : Resource
    {
        ResourceLoadRequest request = this.resourceMap.GetData(key);
        Resource data = request != null ? request.GetAsset() : null;
        if (data == null)
        {
            data = this.AcquireFallbackResource<T>();
            if (data == null)
            {
                EssentialCore.Logger.Error($"Resource was not loaded or registered: {key}");
                return null;
            }
        }

        return this.BuildReference<T>(key, data);
    }

    public bool TryAcquireResource<T>(ResourceKey key, out ResourceReference<T> reference)
        where T : Resource
    {
        reference = null;
        ResourceLoadRequest request = this.resourceMap.GetData(key);
        Resource data = request != null ? request.GetAsset() : null;
        if (data == null)
        {
            return false;
        }

        reference = this.BuildReference<T>(key, data);
        return reference != null;
    }

    public ResourceLoadRequest LoadAsync(ResourceKey key)
    {
        ResourceLoadRequest request = this.resourceMap.GetData(key);

        if (request == null)
        {
            request = DoLoad(new ResourceLoadInfo(key, ResourceLoadFlags.Cache));
            this.resourceMap.SetData(key, request);
        }

        return request;
    }

    public void ReleaseResource<T>(ResourceReference<T> reference)
        where T : Resource
    {
        this.DecreaseResourceRefCount(reference.Key);
    }

    public bool ContinueLoad()
    {
        IList<ResourceLoadRequest> finishedRequests = this.requestPool.GetFinishedRequests();
        if (finishedRequests != null)
        {
            foreach (ResourceLoadRequest request in finishedRequests)
            {
                this.FinalizeLoadResource(request);
            }
        }

        int consecutiveSyncCalls = 0;
        while (this.currentPendingLoads.Count > 0 && this.requestPool.HasFreeSlot())
        {
            ResourceLoadInfo info = this.currentPendingLoads.Dequeue();

            if (this.resourceMap.HasData(info.Key))
            {
                // This resource was already loaded, continue
                return true;
            }

            if ((info.Flags & ResourceLoadFlags.Sync) != 0)
            {
                // This resource is a forced sync load
                this.DoLoadImmediate(info);
                consecutiveSyncCalls++;

                if (consecutiveSyncCalls > MaxConsecutiveSyncCallsInAsync)
                {
                    // Give a frame for the UI to catch up, we probably load a lot of tiny resources
                    return true;
                }

                continue;
            }

            if (this.ResourceLoading != null)
            {
                this.ResourceLoading(info);
            }

            this.requestPool.AddRequest(DoLoad(info));
        }

        return this.currentPendingLoads.Count > 0 || this.requestPool.HasPendingRequests();
    }

    public void LoadImmediate()
    {
        if (this.currentPendingLoads.Count <= 0)
        {
            return;
        }

        int resourceCount = this.currentPendingLoads.Count;
        while (this.currentPendingLoads.Count > 0)
        {
            ResourceLoadInfo info = this.currentPendingLoads.Dequeue();
            if (!this.resourceMap.HasData(info.Key))
            {
                this.DoLoadImmediate(info);
            }
        }

        EssentialCore.Logger.Info($"Immediate! Loaded {resourceCount} resources in {-1}ms");
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static ResourceLoadRequest DoLoad(ResourceLoadInfo info)
    {
        Error request = ResourceLoader.LoadThreadedRequest(info.Key.Path, info.Key.Type.ToString());
        return new ResourceLoadRequest(info, request);
    }

    private ResourceReference<T> BuildReference<T>(ResourceKey key, Resource data)
        where T : Resource
    {
        if (!(data is T))
        {
            EssentialCore.Logger.Error($"Type requested {TypeCache<T>.Value} did not match the registered key type {key.Type} for {key}");
            return null;
        }

        var reference = new ResourceReference<T>(key, (T)data, this);
        this.IncreaseResourceRefCount(key);
        return reference;
    }

    private void DoLoadImmediate(ResourceLoadInfo info)
    {
        if (this.ResourceLoading != null)
        {
            this.ResourceLoading(info);
        }

        Resource result = LoadImmediate(info.Key);

        var request = new ResourceLoadRequest(info, result);
        this.FinalizeLoadResource(request);
    }

    private void IncreaseResourceRefCount(ResourceKey key)
    {
        if (!this.referenceCount.ContainsKey(key))
        {
            this.referenceCount.Add(key, 0);
        }

        this.referenceCount[key]++;
    }

    private void DecreaseResourceRefCount(ResourceKey key)
    {
        if (!this.referenceCount.ContainsKey(key))
        {
            return;
        }

        this.referenceCount[key]--;
        if (this.referenceCount[key] <= 0)
        {
            this.referenceCount.Remove(key);
        }
    }

    private void FinalizeLoadResource(ResourceLoadRequest request)
    {
        Resource data = request.GetAsset();

        if (data == null)
        {
            EssentialCore.Logger.Warn($"Loading {request.Info.Key} returned null data");
            return;
        }

        if (this.EnableHistory)
        {
            if (this.history.ContainsKey(request.Info.Key))
            {
                this.history[request.Info.Key] += 1;
            }
            else
            {
                this.history.Add(request.Info.Key, 1);
            }
        }

        this.resourceMap.SetData(request.Info.Key, request);

        this.ResourcesLoaded++;
        if (this.ResourceLoaded != null)
        {
            this.ResourceLoaded(request.Info, 1);
        }
    }

    private Resource AcquireFallbackResource<T>()
    {
        ResourceKey fallbackKey;
        if (this.fallbackResources.TryGetValue(TypeCache<T>.Value, out fallbackKey))
        {
            ResourceLoadRequest request = this.resourceMap.GetData(fallbackKey);
            Resource data = request != null ? request.GetAsset() : null;
            return data;
        }

        return null;
    }
}
