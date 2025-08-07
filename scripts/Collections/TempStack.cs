namespace Craiel.Essentials.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;

public struct TempStack<T>  : IDisposable, IEnumerable<T>
{
    private const int DefaultCapacity = 100;

    public static readonly TempStack<T> Invalid = new();

    private static readonly Queue<Stack<T>> ReadyQueue = new(2);

    private Stack<T> inner;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public TempStack(Stack<T> inner)
    {
        this.inner = inner;
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public Stack<T> Stack => this.inner;

    public int Count => this.inner.Count;
    
    public static TempStack<T> Allocate()
    {
        lock (ReadyQueue)
        {
            return ReadyQueue.Count == 0
                ? new TempStack<T>(new Stack<T>(DefaultCapacity))
                : new TempStack<T>(ReadyQueue.Dequeue());
        }
    }

    public static TempStack<T> Allocate(IEnumerable<T> entries)
    {
        lock (ReadyQueue)
        {
            if (ReadyQueue.Count == 0)
            {
                return new TempStack<T>(new Stack<T>(entries));
            }

            Stack<T> inner = ReadyQueue.Dequeue();
            inner.Clear();

            if (entries != null)
            {
                foreach (T entry in entries)
                {
                    inner.Push(entry);
                }
            }

            return new TempStack<T>(inner);
        }
    }

    public void Dispose()
    {
        if (this.inner != null)
        {
            lock (ReadyQueue)
            {
                this.inner.Clear();
                ReadyQueue.Enqueue(Stack);
                this.inner = null;
            }
        }
    }

    public void Push(T entry)
    {
        this.inner.Push(entry);
    }

    public T Pop()
    {
        return this.inner.Pop();
    }

    public bool TryPop(out T entry)
    {
        return this.inner.TryPop(out entry);
    }

    public void PushRange(IEnumerable<T> entries)
    {
        this.inner.PushRange(entries);
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