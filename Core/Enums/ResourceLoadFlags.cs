namespace Craiel.Essentials.Runtime.Enums;

using System;

[Flags]
public enum ResourceLoadFlags
{
    None = 0,
    Sync = 1 << 0,
    Cache = 1 << 1,
}