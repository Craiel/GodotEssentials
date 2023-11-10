namespace Craiel.Essentials.Collections;

using System;
using System.Collections;
using System.Collections.Generic;

public struct TempHashSet<T> : IDisposable, IEnumerable<T>
{
    public static readonly TempHashSet<T> Invalid = new TempHashSet<T>();

    private static readonly Queue<HashSet<T>> ReadyQueue = new Queue<HashSet<T>>(2);

    private HashSet<T> inner;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TempHashSet(HashSet<T> inner)
    {
        this.inner = inner;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public HashSet<T> Set => this.inner;

    public int Count => this.inner.Count;

    public static TempHashSet<T> Allocate()
    {
        lock (ReadyQueue)
        {
            return ReadyQueue.Count == 0
                ? new TempHashSet<T>(new HashSet<T>())
                : new TempHashSet<T>(ReadyQueue.Dequeue());
        }
    }

    public static TempHashSet<T> Allocate(IEnumerable<T> entries)
    {
        lock (ReadyQueue)
        {
            if (ReadyQueue.Count == 0)
            {
                return new TempHashSet<T>(new HashSet<T>(entries));
            }

            HashSet<T> inner = ReadyQueue.Dequeue();
            inner.Clear();

            if (entries != null)
            {
                foreach (T entry in entries)
                {
                    inner.Add(entry);
                }
            }

            return new TempHashSet<T>(inner);
        }
    }

    public void Dispose()
    {
        if (this.inner != null)
        {
            lock (ReadyQueue)
            {
                this.inner.Clear();
                ReadyQueue.Enqueue(this.Set);
                this.inner = null;
            }
        }
    }

    public bool Contains(T entry)
    {
        return this.inner.Contains(entry);
    }

    public void Add(T entry)
    {
        this.inner.Add(entry);
    }

    public bool Remove(T entry)
    {
        return this.inner.Remove(entry);
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return this.inner.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}