namespace Craiel.Essentials.Extensions;

using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using Godot.Collections;
using SaveLoad;
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
        SetStructDataInternal(target, structValue, typeof(T));
    }

    public static T GetStructData<T>(this Dictionary source) where T : struct
    {
        return (T)GetStructDataInternal(source, typeof(T));
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void SetStructDataInternal(Dictionary target, object structValue, Type structType)
    {
        var fields = GetPersistentFields(structType);
        foreach (var (attribute, fieldInfo) in fields)
        {
            var value = fieldInfo.GetValue(structValue);
            var fieldType = fieldInfo.FieldType;

            if (fieldType.IsValueType && !fieldType.IsPrimitive && !fieldType.IsEnum && fieldType != typeof(string))
            {
                var nestedFields = GetPersistentFields(fieldType);
                if (nestedFields.Count > 0)
                {
                    var nestedDict = new Dictionary();
                    SetStructDataInternal(nestedDict, value, fieldType);
                    target[attribute.Key] = nestedDict;
                    continue;
                }
            }

            target[attribute.Key] = Variant.From(value);
        }
    }

    private static object GetStructDataInternal(Dictionary source, Type structType)
    {
        var result = Activator.CreateInstance(structType);
        var fields = GetPersistentFields(structType);

        foreach (var (attribute, fieldInfo) in fields)
        {
            var fieldType = fieldInfo.FieldType;

            if (fieldType.IsValueType && !fieldType.IsPrimitive && !fieldType.IsEnum && fieldType != typeof(string))
            {
                var nestedFields = GetPersistentFields(fieldType);
                if (nestedFields.Count > 0)
                {
                    var nestedDict = source[attribute.Key].AsGodotDictionary();
                    var nestedValue = GetStructDataInternal(nestedDict, fieldType);
                    fieldInfo.SetValue(result, nestedValue);
                    continue;
                }
            }

            var variant = source[attribute.Key];
            var value = ConvertFromVariant(variant, fieldType);
            fieldInfo.SetValue(result, value);
        }

        return result;
    }

    private static List<(PersistentFieldAttribute, FieldInfo)> GetPersistentFields(Type type)
    {
        var result = new List<(PersistentFieldAttribute, FieldInfo)>();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<PersistentFieldAttribute>();
            if (attribute != null)
            {
                result.Add((attribute, field));
            }
        }

        return result;
    }

    private static object ConvertFromVariant(Variant variant, Type fieldType)
    {
        if (fieldType == typeof(bool)) return variant.AsBool();
        if (fieldType == typeof(int)) return variant.AsInt32();
        if (fieldType == typeof(float)) return variant.AsSingle();
        if (fieldType == typeof(double)) return variant.AsDouble();
        if (fieldType == typeof(string)) return variant.AsString();
        if (fieldType == typeof(ulong)) return variant.AsUInt64();
        if (fieldType == typeof(ushort)) return variant.AsUInt16();
        if (fieldType == typeof(byte)) return variant.AsByte();
        if (fieldType == typeof(long)) return variant.AsInt64();
        if (fieldType == typeof(short)) return variant.AsInt16();
        if (fieldType.IsEnum) return Enum.ToObject(fieldType, variant.AsInt32());

        return variant.Obj;
    }
}
