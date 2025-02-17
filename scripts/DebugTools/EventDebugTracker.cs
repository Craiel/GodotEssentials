namespace Craiel.Essentials.DebugTools;

using System;
using System.Collections.Generic;
using System.Linq;

public class EventDebugTracker<T>
{
    public readonly IDictionary<Type, int> Sends = new Dictionary<Type, int>();
    public readonly IDictionary<Type, int> Receives = new Dictionary<Type, int>();
    public readonly IDictionary<Type, double> Time = new Dictionary<Type, double>();

    public int TotalSent;
    public int TotalReceived;
    public double TotalTime;
    
    public void Track<TSpecific>(int sends, int receives, double time)
        where TSpecific : T
    {
#if DEBUG
        this.TotalReceived += receives;
        this.TotalSent += sends;
        this.TotalTime += time;
        
        var type = typeof(TSpecific);
        if (this.Sends.TryAdd(type, 0))
        {
            this.Receives.Add(type, 0);
            this.Time.Add(type, 0);
        }
        
        this.Sends[type] += sends;
        this.Receives[type] += receives;
        this.Time[type] += time;
#endif
    }

    public void Clear()
    {
        this.TotalReceived = 0;
        this.TotalSent = 0;
        this.TotalTime = 0;
        
        this.Sends.Clear();
        this.Receives.Clear();
        this.Time.Clear();
    }

    public int GetReceivedCount(Type type)
    {
        return this.Receives[type];
    }

    public double GetTime(Type type)
    {
        return this.Time[type];
    }

    public int GetSentCount(Type type)
    {
        return this.Sends[type];
    }

    public void GetTopBySent(int count, out IList<Type> results)
    {
        results = this.Sends.OrderBy(x => x.Value).Take(count).Select(x => x.Key).ToList();
    }
    
    public void GetTopByReceived(int count, out IList<Type> results)
    {
        results = this.Receives.OrderByDescending(x => x.Value).Take(count).Select(x => x.Key).ToList();
    }
}