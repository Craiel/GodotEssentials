namespace Craiel.Essentials.Commands;

using System;
using Craiel.Essentials.Contracts;

public struct EnumPayload<T> : IGameCommandPayload
    where T : Enum
{
// -------------------------------------------------------------------
// Constructor
// -------------------------------------------------------------------
    public EnumPayload(T value)
    {
        this.Value = value;
    }

// -------------------------------------------------------------------
// Public
// -------------------------------------------------------------------
    public T Value;
}