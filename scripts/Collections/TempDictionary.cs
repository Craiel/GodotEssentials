namespace Craiel.Essentials.Collections;

using System;
using System.Collections;
using System.Collections.Generic;

public struct TempDictionary<TK, TV> : IDisposable, IEnumerable<TK>
{
    private const int DefaultCapacity = 100;

    public static readonly TempDictionary<TK, TV> Invalid = new TempDictionary<TK, TV>();

    private static readonly Queue<Dictionary<TK, TV>> ReadyQueue = new Queue<Dictionary<TK, TV>>(2);

    private Dictionary<TK, TV> inner;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TempDictionary(Dictionary<TK, TV> inner)
    {
        this.inner = inner;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public Dictionary<TK, TV> Dictionary => this.inner;

    public int Count => this.inner.Count;

    public static TempDictionary<TK, TV> Allocate()
    {
        lock (ReadyQueue)
        {
            return ReadyQueue.Count == 0
                ? new TempDictionary<TK, TV>(new Dictionary<TK, TV>(DefaultCapacity))
                : new TempDictionary<TK, TV>(ReadyQueue.Dequeue());
        }
    }

    public void Dispose()
    {
        if (this.inner != null)
        {
            lock (ReadyQueue)
            {
                this.inner.Clear();
                ReadyQueue.Enqueue(Dictionary);
                this.inner = null;
            }
        }
    }

    public void Add(TK key, TV value)
    {
        this.inner.Add(key, value);
    }

    public bool ContainsKey(TK key)
    {
        return this.inner.ContainsKey(key);
    }

    public IEnumerator<TK> GetEnumerator()
    {
        return this.inner.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}