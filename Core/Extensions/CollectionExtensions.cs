namespace Craiel.Essentials.Runtime.Extensions;

using System.Collections.Generic;

public static class CollectionExtensions
{
    public static void Shuffle<T> (this T[] array) where T : struct
    {
        int iL = array.Length;
        while (iL > 1)
        {
            int iS = EssentialCore.Random.RandiRange(0, iL--);
            (array[iL], array[iS]) = (array[iS], array[iL]);
        }
    }
    
    public static T RandomEntry<T>(this T[] array)
    {
        return array.Length > 0
            ? array[EssentialCore.Random.RandiRange(0, array.Length)]
            : default;
    }
    
    public static T RandomEntry<T>(this List<T> array)
    {
        return array.Count > 0 
            ? array[EssentialCore.Random.RandiRange(0, array.Count)] 
            : default;
    }
    
    public static void AddRange<T>(this IList<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Add(entry);
        }
    }

    public static void RemoveRange<T>(this IList<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Remove(entry);
        }
    }

    public static void AddRange<T>(this HashSet<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Add(entry);
        }
    }

    public static void RemoveRange<T>(this HashSet<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Remove(entry);
        }
    }

    public static void EnqueueRange<T>(this Queue<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Enqueue(entry);
        }
    }

    public static void PushRange<T>(this Stack<T> target, IEnumerable<T> source)
    {
        foreach (T entry in source)
        {
            target.Push(entry);
        }
    }

    public static void AddRange<T, TN>(this IDictionary<T, TN> target, IDictionary<T, TN> source)
    {
        foreach (KeyValuePair<T, TN> entry in source)
        {
            target.Add(entry.Key, entry.Value);
        }
    }
    
    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        return array == null || array.Length == 0;
    }
    
    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }
    
    public static bool IsNullOrEmpty<T>(this HashSet<T> hashset)
    {
        return hashset == null || hashset.Count == 0;
    }
}
