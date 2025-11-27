namespace Craiel.Essentials.Extensions;

using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Utils;

public static class CollectionExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
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

    public static void SetStructData<T>(this Dictionary target, T structValue) where T : struct
    {
        foreach (var persistentField in TypeDef<T>.PersistentFields)
        {
            var value = persistentField.Value.GetValue(structValue);
            var variant = Variant.From(value);
            target[persistentField.Key.Key] = variant;
        }
    }

    public static T GetStructData<T>(this Dictionary source) where T : struct
    {
        T result = new T();
        object boxed = result;
        foreach (var persistentField in TypeDef<T>.PersistentFields)
        {
            var variant = source[persistentField.Key.Key];
            var value = ConvertFromVariant(variant, persistentField.Value.FieldType);
            persistentField.Value.SetValue(boxed, value);
        }

        return (T)boxed;
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static object ConvertFromVariant(Variant variant, System.Type fieldType)
    {
        if (fieldType == typeof(bool)) return variant.AsBool();
        if (fieldType == typeof(int)) return variant.AsInt32();
        if (fieldType == typeof(float)) return variant.AsSingle();
        if (fieldType == typeof(double)) return variant.AsDouble();
        if (fieldType == typeof(string)) return variant.AsString();
        if (fieldType.IsEnum) return System.Enum.ToObject(fieldType, variant.AsInt32());

        return variant.Obj;
    }
}
