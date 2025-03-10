namespace Craiel.Essentials.DB;

using System.Collections.Generic;
using System.IO;

public static class CrossDBLookup
{
    private static readonly IDictionary<IGameDataId, IGameDataEntry> entries = new Dictionary<IGameDataId, IGameDataEntry>();
    
    public static void Register(IGameDataId id, IGameDataEntry entry)
    {
        if (!entries.TryAdd(id, entry))
        {
            throw new InvalidDataException("Duplicate ID: " + id);
        }
    }

    public static IGameDataEntry Get(IGameDataId id)
    {
        if (entries.TryGetValue(id, out var entry))
        {
            return entry;
        }

        throw new InvalidDataException("No such DB Entry: " + id);
    }
}