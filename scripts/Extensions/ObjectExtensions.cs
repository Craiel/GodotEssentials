﻿namespace Craiel.Essentials.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Utils;

public static class ObjectExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static bool DeepCompare(this object a, object b)
    {
        // Check the standard implementations first
        if (a == b || ReferenceEquals(a, b) || Equals(a, b))
        {
            return true;
        }

        // Now check some assumptions
        if (a == null || b == null)
        {
            return false;
        }

        Type compareType = a.GetType();
        if (compareType != b.GetType())
        {
            return false;
        }

        if (compareType.BaseType == TypeDef<ValueType>.Value
            || compareType.BaseType == TypeDef<Enum>.Value
            || compareType == TypeDef<string>.Value)
        {
            // Value types, strings and enums should have match at this point
            return false;
        }

        IEnumerable enumerableA = a as IEnumerable;
        if (enumerableA != null)
        {
            return enumerableA.DeepCompareEnumerable((IEnumerable) b);
        }

        // Now do the actual deep scan
        PropertyInfo[] properties = a.GetType().GetProperties();
        for (var i = 0; i < properties.Length; i++)
        {
            PropertyInfo info = properties[i];

            object valueA = info.GetValue(a, BindingFlags.Public, null, null, CultureInfo.InvariantCulture);
            object valueB = info.GetValue(b, BindingFlags.Public, null, null, CultureInfo.InvariantCulture);
            if (!valueA.DeepCompare(valueB))
            {
#if DEBUG
                EssentialCore.Logger.Warn($"DeepCompare failed on property {info.Name} of {compareType.Name}");
#endif

                return false;
            }
        }

        return true;
    }

    public static bool DeepCompareEnumerable(this IEnumerable a, IEnumerable b)
    {
        IList<object> entriesA = new List<object>();
        foreach (object entry in a)
        {
            entriesA.Add(entry);
        }

        IList<object> entriesB = new List<object>();
        foreach (object entry in b)
        {
            entriesB.Add(entry);
        }

        if (entriesA.Count != entriesB.Count)
        {
            return false;
        }

        for (var i = 0; i < entriesA.Count; i++)
        {
            if (!entriesA[i].DeepCompare(entriesB[i]))
            {
                return false;
            }
        }

        return true;
    }
}
