namespace Craiel.Essentials.Pool;

using System;
using Contracts;
using Godot;
using Resource;

public class NodePool<T> : BasePool<T>
    where T : Node, IPoolable
{
    private const int DefaultSize = 20;
    
    private T[] activeEntries;

    private int nextFreeSlot;
    
    private PackedScene prefab;
    private Func<T, bool> activeUpdateCallback;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public NodePool(int capacity = DefaultSize, int maxCapacity = int.MaxValue)
        : base(capacity, maxCapacity)
    {
        this.activeEntries = new T[capacity];
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public int ActiveCount { get; private set; }

    private void Initialize(Func<T, bool> updateCallback)
    {
        this.activeUpdateCallback = updateCallback;
    }
    
    public void Initialize(ResourceKey poolPrefabKey, Func<T, bool> updateCallback)
    {
        this.Initialize(updateCallback);

        this.prefab = poolPrefabKey.LoadManaged<PackedScene>();
        if (this.prefab == null)
        {
            throw new InvalidOperationException("Resource could not be loaded: " + poolPrefabKey);
        }
    }
    
    public void Initialize(PackedScene poolPrefab, Func<T, bool> updateCallback)
    {
        this.Initialize(updateCallback);

        this.prefab = poolPrefab;
    }
    
    public void Update(double delta)
    {
        for (var i = 0; i < this.activeEntries.Length; i++)
        {
            if (this.activeEntries[i] == null)
            {
                continue;
            }

            if (!this.UpdateActiveEntry(this.activeEntries[i]))
            {
                this.Free(this.activeEntries[i]);
                this.activeEntries[i] = null;
                this.ActiveCount--;

                if (this.nextFreeSlot > i)
                {
                    this.nextFreeSlot = i;
                }
            }
        }
    }
    
    public override T Obtain()
    {
        T entry = base.Obtain();
        this.activeEntries[this.nextFreeSlot] = entry;
        this.FindNextFreeSlot();
        this.ActiveCount++;

        return entry;
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override T NewObject()
    {
        return this.prefab.Instantiate<T>();
    }
    
    protected override void Reset(T entry)
    {
        for (var i = 0; i < this.activeEntries.Length; i++)
        {
            if (this.activeEntries[i] != null && this.activeEntries[i] == entry)
            {
                this.activeEntries[i] = null;
                this.ActiveCount--;

                if (this.nextFreeSlot > i)
                {
                    this.nextFreeSlot = i;
                }
                
                break;
            }
        }

        base.Reset(entry);
    }

    protected bool UpdateActiveEntry(T entry)
    {
        return this.activeUpdateCallback(entry);
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void FindNextFreeSlot()
    {
        for (var n = 0; n < this.activeEntries.Length; n++)
        {
            this.nextFreeSlot++;
            if (this.nextFreeSlot == this.activeEntries.Length)
            {
                this.nextFreeSlot = 0;
            }

            if (this.activeEntries[this.nextFreeSlot] == null)
            {
                return;
            }
        }

        this.nextFreeSlot = this.activeEntries.Length;
        Array.Resize(ref this.activeEntries, this.activeEntries.Length + DefaultSize);
    }
}
