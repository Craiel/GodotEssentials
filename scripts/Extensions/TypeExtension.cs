namespace Craiel.Essentials.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utils;

public static class TypeExtension
{
    public static bool Implements<T>(this Type type)
        where T : class
    {
        Type targetType = TypeDef<T>.Value;
        if (!targetType.IsInterface)
        {
            throw new InvalidOperationException("Interface type expected for Implements call");
        }

        return targetType.IsAssignableFrom(type);
    }

    public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
    }

    public static T GetCustomAttribute<T>(this Type type, bool inherit = false)
        where T : Attribute
    {
        object[] results = type.GetCustomAttributes(type, inherit);
        if (results.Length <= 0)
        {
            return null;
        }

        if (results.Length > 1)
        {
            throw new InvalidOperationException("Expected only one attribute but found " + results.Length);
        }

        return results[0] as T;
    }

    public static bool IsNullable(this Type type)
    {
        return type.IsClass || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
    }

    public static object GetDefault(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static Type GetActualType(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type;
        }

        var genericArgs = type.GetGenericArguments();
        if (genericArgs.Length != 1)
        {
            throw new InvalidOperationException("Multiple candidates, can not return a single actual type");
        }

        return genericArgs[0];
    }

    public static object ConvertValue(this Type type, object source)
    {
        bool isNullable = type.IsClass;
        Type targetType = type;
        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            isNullable = true;
            targetType = targetType.GetGenericArguments()[0];
        }

        if (source == null || source is DBNull)
        {
            if (isNullable)
            {
                return default(Type);
            }

            throw new InvalidOperationException("Target type is not nullable, source value is invalid");
        }

        if (source.GetType() == type)
        {
            return source;
        }

        if (targetType == TypeDef<int>.Value)
        {
            return Convert.ToInt32(source);
        }

        if (targetType == TypeDef<uint>.Value)
        {
            return Convert.ToUInt32(source);
        }

        if (targetType == TypeDef<long>.Value)
        {
            return Convert.ToInt64(source);
        }

        if (targetType == TypeDef<bool>.Value)
        {
            return Convert.ToBoolean(source);
        }

        if (targetType == TypeDef<float>.Value)
        {
            return Convert.ToSingle(source);
        }

        if (targetType == TypeDef<DateTime>.Value)
        {
            return Convert.ToDateTime(source);
        }

        if (targetType.IsEnum)
        {
            string name;
            if (source is string)
            {
                name = (string)source;
            }
            else
            {
                name = Enum.GetName(targetType, source);
            }

            // In case of null we return default
            if (name == null)
            {
                return Activator.CreateInstance(targetType);
            }

            return Enum.Parse(targetType, name);
        }

        throw new NotImplementedException(string.Format("Can not get Typed value of {0} for target type {1}", source.GetType(), targetType));
    }

    public static IDictionary<string, T> GetStaticProperties<T>(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == TypeDef<T>.Value)
                .ToDictionary(f => f.Name, f => (T)f.GetValue(null));
    }
}
