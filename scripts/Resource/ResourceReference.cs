﻿namespace Craiel.Essentials.Resource;

using System;
using Godot;

public class ResourceReference<T> : IDisposable
    where T : Resource
{
    private readonly ResourceProvider provider;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ResourceReference(ResourceKey key, T data, ResourceProvider provider)
    {
        this.Key = key;
        this.Data = data;
        this.provider = provider;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public ResourceKey Key { get; private set; }

    public T Data { get; private set; }

    public void Dispose()
    {
        this.Dispose(true);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            this.provider.ReleaseResource(this);
        }
    }
}
