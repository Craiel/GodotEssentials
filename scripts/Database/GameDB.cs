namespace Craiel.Essentials.DB;

using System.Collections.Generic;
using System.IO;

public class GameDB<T, TI>
    where T: GameDataEntry<TI>
    where TI: IGameDataId
{
    private readonly IDictionary<TI, T> lookup = new Dictionary<TI, T>();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public int Count => Entries.Count;
    
    public readonly IList<T> Entries = new List<T>();
    
    public void Clear()
    {
        this.Entries.Clear();
        this.lookup.Clear();
    }

    public bool Contains(TI id)
    {
        return this.lookup.ContainsKey(id);
    }

    public bool TryGet(TI id, out T entry)
    {
        return this.lookup.TryGetValue(id, out entry);
    }

    public T Get(TI id)
    {
        if (!this.lookup.TryGetValue(id, out T result))
        {
            throw new InvalidDataException("No such DB entry: " + id + " (" + typeof(T) + ")");
        }

        return result;
    }
    
    public void Register(T data)
    {
        if (this.lookup.ContainsKey(data.Id))
        {
            throw new InvalidDataException("Duplicate ID: " + data.Id + " (" + typeof(T) + ")");
        }
        
        this.Entries.Add(data);
        this.lookup.Add(data.Id, data);
        
        CrossDBLookup.Register(data.Id, data);
    }
}