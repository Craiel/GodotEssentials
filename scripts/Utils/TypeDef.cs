namespace Craiel.Essentials.Utils;

using System;
using System.Reflection;
using SaveLoad;

public static class TypeDef<T>
{
    private static IDictionary<PersistentFieldAttribute, FieldInfo> persistentFieldCache;
    
    public static readonly Type Value = typeof(T);
    public static readonly int Hash = Value.GetHashCode();

    public static IDictionary<PersistentFieldAttribute, FieldInfo> PersistentFields
    {
        get
        {
            if (persistentFieldCache == null)
            {
                persistentFieldCache = new Dictionary<PersistentFieldAttribute, FieldInfo>();
                
                var type = typeof(T);
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    var attribute = field.GetCustomAttribute<PersistentFieldAttribute>();
                    if (attribute == null)
                    {
                        continue;
                    }

                    persistentFieldCache.Add(attribute, field);
                }
            }

            return persistentFieldCache;
        }
    }
}