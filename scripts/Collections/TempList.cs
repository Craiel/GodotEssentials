namespace Craiel.Essentials.Collections;

using System;
using System.Collections;
using System.Collections.Generic;

public struct TempList<T> : IDisposable, IEnumerable<T>
{
    private const int DefaultCapacity = 100;

    public static readonly TempList<T> Invalid = new();

    private static readonly Queue<List<T>> ReadyQueue = new(2);

    private List<T> inner;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TempList(List<T> inner)
    {
        this.inner = inner;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public List<T> List => this.inner;

    public int Count => this.inner.Count;
    
    public T this[int index]
    {
        get => this.inner[index];
        set => this.inner[index] = value;
    }

    public static TempList<T> Allocate()
    {
        lock (ReadyQueue)
        {
            return ReadyQueue.Count == 0
                ? new TempList<T>(new List<T>(DefaultCapacity))
                : new TempList<T>(ReadyQueue.Dequeue());
        }
    }

    public static TempList<T> Allocate(IEnumerable<T> entries)
    {
        lock (ReadyQueue)
        {
            if (ReadyQueue.Count == 0)
            {
                return new TempList<T>(new List<T>(entries));
            }

            List<T> inner = ReadyQueue.Dequeue();
            inner.Clear();

            if (entries != null)
            {
                foreach (T entry in entries)
                {
                    inner.Add(entry);
                }
            }

            return new TempList<T>(inner);
        }
    }

    public void Dispose()
    {
        if (this.inner != null)
        {
            lock (ReadyQueue)
            {
                this.inner.Clear();
                ReadyQueue.Enqueue(List);
                this.inner = null;
            }
        }
    }

    public void Add(T entry)
    {
        this.inner.Add(entry);
    }

    public void AddRange(IEnumerable<T> entries)
    {
        this.inner.AddRange(entries);
    }
    
    public void AddRangeInclusive(IEnumerable<T> entries)
    {
        foreach (T entry in entries)
        {
            if (this.inner.Contains(entry))
            {
                continue;
            }
            
            this.inner.Add(entry);
        }
    }

    public void Clear()
    {
        this.inner.Clear();
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