namespace Craiel.Essentials.Runtime.Resource;

using System;
using Contracts;
using Enums;
using Godot;

public class ResourceLoadRequest : IResourceRequest
{
    private readonly Resource asset;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ResourceLoadRequest(ResourceLoadInfo info, Error threadedStatus)
    {
        if (threadedStatus != Error.Ok)
        {
            throw new InvalidOperationException("Resource Load Failed: " + threadedStatus);
        }
        
        this.Mode = ResourceLoadMode.Async;
    }
    
    public ResourceLoadRequest(ResourceLoadInfo info, Resource asset)
        : this(info)
    {
        this.asset = asset;
        this.Mode = ResourceLoadMode.Assigned;
    }

    protected ResourceLoadRequest(ResourceLoadInfo info)
    {
        this.Info = info;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public ResourceLoadInfo Info { get; private set; }

    public ResourceLoadMode Mode { get; private set; }
    
    public bool IsDone
    {
        get
        {
            switch (this.Mode)
            {
                case ResourceLoadMode.Assigned:
                {
                    return true;
                }

                case ResourceLoadMode.Async:
                {
                    return ResourceLoader.LoadThreadedGetStatus(this.Info.Key.Path) == ResourceLoader.ThreadLoadStatus.Loaded;
                }

                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }

    public T GetAsset<T>() where T : Resource
    {
        switch (this.Mode)
        {
            case ResourceLoadMode.Assigned:
                {
                    return this.asset as T;
                }

            default:
                {
                    throw new NotImplementedException();
                }
        }
    }

    public Resource GetAsset()
    {
        switch (this.Mode)
        {
                case ResourceLoadMode.Assigned:
                {
                    return this.asset;
                }

            default:
                {
                    throw new NotImplementedException();
                }
        }
    }
}
