namespace Craiel.Essentials.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;

public struct TempQueue<T>  : IDisposable, IEnumerable<T>
{
    private const int DefaultCapacity = 100;

    public static readonly TempQueue<T> Invalid = new();

    private static readonly Queue<Queue<T>> ReadyQueue = new(2);

    private Queue<T> inner;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TempQueue(Queue<T> inner)
    {
        this.inner = inner;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public Queue<T> Queue => this.inner;

    public int Count => this.inner.Count;
    
    public static TempQueue<T> Allocate()
    {
        lock (ReadyQueue)
        {
            return ReadyQueue.Count == 0
                ? new TempQueue<T>(new Queue<T>(DefaultCapacity))
                : new TempQueue<T>(ReadyQueue.Dequeue());
        }
    }

    public static TempQueue<T> Allocate(IEnumerable<T> entries)
    {
        lock (ReadyQueue)
        {
            if (ReadyQueue.Count == 0)
            {
                return new TempQueue<T>(new Queue<T>(entries));
            }

            Queue<T> inner = ReadyQueue.Dequeue();
            inner.Clear();

            if (entries != null)
            {
                foreach (T entry in entries)
                {
                    inner.Enqueue(entry);
                }
            }

            return new TempQueue<T>(inner);
        }
    }

    public void Dispose()
    {
        if (this.inner != null)
        {
            lock (ReadyQueue)
            {
                this.inner.Clear();
                ReadyQueue.Enqueue(Queue);
                this.inner = null;
            }
        }
    }

    public void Enqueue(T entry)
    {
        this.inner.Enqueue(entry);
    }

    public T Dequeue()
    {
        return this.inner.Dequeue();
    }

    public bool TryDequeue(out T entry)
    {
        return this.inner.TryDequeue(out entry);
    }

    public void EnqueueRange(IEnumerable<T> entries)
    {
        this.inner.EnqueueRange(entries);
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