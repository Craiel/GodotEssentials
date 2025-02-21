namespace Craiel.Essentials.Nodes;

using System;
using Godot;

public partial class SingletonNode<T> : Node
    where T : SingletonNode<T>
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static T Instance { get; private set; }

    public override void _EnterTree()
    {
        base._EnterTree();

        if (Instance != null)
        {
            throw new InvalidOperationException("Duplicate Instances of Singleton: " + typeof(T));
        }

        Instance = this as T;
        if (Instance == null)
        {
            throw new InvalidOperationException("Singleton was declared wrong");
        }
    }
}