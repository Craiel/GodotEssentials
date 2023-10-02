namespace Craiel.Essentials.Threading;

using System;
using System.Collections.Generic;
using EngineCore;

public class SynchronizationDispatcher : IGameModule
{
    private static readonly Queue<Action> Tasks = new Queue<Action>();
    private static readonly IList<Action> TaskCache = new List<Action>();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void InvokeLater(Action task)
    {
        lock (Tasks)
        {
            Tasks.Enqueue(task);
        }
    }

    public void Initialize()
    {
    }

    public void Update(double delta)
    {
        TaskCache.Clear();
        lock (Tasks)
        {
            while (Tasks.Count > 0)
            {
                TaskCache.Add(Tasks.Dequeue());
            }
        }

        foreach (Action action in TaskCache)
        {
            action.Invoke();
        }
    }

    public void Destroy()
    {
    }
}
