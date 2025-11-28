namespace Craiel.Essentials.Utils;

using System;

public static class TypeDef<T>
{
    public static readonly Type Value = typeof(T);
    public static readonly int Hash = Value.GetHashCode();
}