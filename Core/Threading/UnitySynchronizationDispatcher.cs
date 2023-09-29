namespace Craiel.Essentials.Runtime.Threading;

using System;
using System.Collections.Generic;
using Singletons;

public class UnitySynchronizationDispatcher : GodotSingleton<UnitySynchronizationDispatcher>
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

    [UsedImplicitly]
    public void FixedUpdate()
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
}
