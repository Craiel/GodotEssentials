namespace Craiel.Essentials.Runtime.Resource;

using System;
using Godot;
using Utils;

public struct ResourceKey
{
    public static readonly ResourceKey Invalid = new ResourceKey();

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public ResourceKey(string path, Type type)
        : this()
    {
        this.Path = path;
        this.Type = type ?? TypeCache<Resource>.Value;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public string Path { get; set; }

    public Type Type { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(this.Path);
    }

    public static ResourceKey Create<T>(string path)
    {
        return new ResourceKey(path, TypeCache<T>.Value);
    }

    public static bool operator ==(ResourceKey rhs, ResourceKey lhs)
    {
        return rhs.Path == lhs.Path
            && rhs.Type == lhs.Type;
    }

    public static bool operator !=(ResourceKey rhs, ResourceKey lhs)
    {
        return !(rhs == lhs);
    }

    public override int GetHashCode()
    {
        return this.Path.GetHashCode();
    }

    public override bool Equals(object other)
    {
        if (other == null || other.GetType() != TypeCache<ResourceKey>.Value)
        {
            return false;
        }

        ResourceKey typed = (ResourceKey)other;
        return this == typed;
    }

    public override string ToString()
    {
        return string.Format("{0} ({1})", this.Path, this.Type);
    }
}
