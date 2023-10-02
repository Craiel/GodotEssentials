namespace Craiel.Essentials.Utils;

using System;
using System.Reflection;
using Collections;

public static class TypeLookup
{
    private static Assembly[] assemblyCache;
    private static Type[] typeCache;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static Type FindType<T>(string value)
    {
        RefreshCache();

        for (var i = 0; i < typeCache.Length; i++)
        {
            var type = typeCache[i];

            if (!TypeCache<T>.Value.IsAssignableFrom(type))
            {
                continue;
            }

            if (type.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return typeCache[i];
            }
        }

        return null;
    }

    public static void InvalidateCache()
    {
        assemblyCache = null;
        typeCache = null;
    }

    private static void RefreshCache()
    {
        if (assemblyCache == null)
        {
            assemblyCache = AppDomain.CurrentDomain.GetAssemblies();

            typeCache = null;
        }

        if (typeCache == null)
        {
            using (var tempList = TempList<Type>.Allocate())
            {
                foreach (Assembly assembly in assemblyCache)
                {
                    try
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            tempList.Add(type);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                typeCache = tempList.List.ToArray();
            }
        }
    }
}