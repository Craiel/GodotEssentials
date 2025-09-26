namespace Craiel.Essentials.DB;

using System.Collections.Generic;
using System.IO;

public static class CrossDBLookup
{
    private static readonly IDictionary<IGameDataId, IGameDataEntry> entries = new Dictionary<IGameDataId, IGameDataEntry>();
    private static readonly IDictionary<GameDataType, IList<IGameDataId>> entryTypeLookup = new Dictionary<GameDataType, IList<IGameDataId>>();
    
    public static void Register(IGameDataId id, IGameDataEntry entry)
    {
        if (!entries.TryAdd(id, entry))
        {
            throw new InvalidDataException("Duplicate ID: " + id);
        }

        if (!entryTypeLookup.TryGetValue(id.Type, out var list))
        {
            list = new List<IGameDataId>();
            entryTypeLookup.Add(id.Type, list);
        }
        
        list.Add(id);
    }

    public static IGameDataEntry Get(IGameDataId id)
    {
        if (entries.TryGetValue(id, out var entry))
        {
            return entry;
        }

        throw new InvalidDataException("No such DB Entry: " + id);
    }

    public static void GetAll(GameDataType type, IList<IGameDataEntry> results)
    {
        if (!entryTypeLookup.TryGetValue(type, out var list))
        {
            return;
        }

        foreach (IGameDataId id in list)
        {
            results.Add(entries[id]);
        }
    }
}